using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elite.Dsd
{
    class ScannerOperation
    {
        private readonly int MaxDegreeOfParallelism = 3;

        private static Dictionary<int, ScannerGroup> scanners = GetDefaultScanners();
        private static Dictionary<int, ScannerGroup> GetDefaultScanners()
        {
            var root = new Dictionary<int, ScannerGroup>();
            var inboundScanners = new ScannerGroup((int)ScannerGroupId.InboundScannerGroup, "InboundScannerGroup") { ScannerList = new List<Scanner>() };
            inboundScanners.ScannerList.AddRange(new[]
            {
                new Scanner((int)ScannerId.Inbound01,"inbound1","200.200.200.68",2112),
                new Scanner((int)ScannerId.Inbound02,"inbound2","200.200.200.78",2112)
            });
            root[inboundScanners.Id] = inboundScanners;

            var outboundScanners = new ScannerGroup((int)ScannerGroupId.OutboundScannerGroup, "OutboundScannerGroup") { ScannerList = new List<Scanner>() };
            outboundScanners.ScannerList.AddRange(new[]
            {
                new Scanner((int)ScannerId.Outbound01,"outbound1","200.200.200.69",2112),
                new Scanner((int)ScannerId.Outbound02,"outbound2","200.200.200.79",2112)
            });
            root[outboundScanners.Id] = outboundScanners;
            return root;
        }
        public Dictionary<int, ScannerGroup> Scanners { get { return scanners; } set { value = scanners; } }

        public Dictionary<int, string> GetScannerBarcodeByGroup(int id)
        {
            //var result = new Dictionary<int, string>();
            var result = new ConcurrentDictionary<int, string>();

            if (Scanners.ContainsKey(id) && Scanners[id].ScannerList != null && Scanners[id].ScannerList.Any())
            {
                Parallel.ForEach(Scanners[id].ScannerList, new ParallelOptions() { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, scanner =>
                {
                    try
                    {
                        if (scanner.Status != TaskStatus.Faulted)
                        {
                            result.TryAdd(scanner.Id, scanner.SendSignalAndGetBarcode());
                            scanner.Status = TaskStatus.RanToCompletion;
                        }
                        else
                        {
                            result.TryAdd(scanner.Id, null);
                        }
                    }
                    catch (Exception e)
                    {
                        result.TryAdd(scanner.Id, null);
                        scanner.Message = e.Message;
                        scanner.Status = TaskStatus.Faulted;
                    }
                });
                Task.WaitAll();
                return result.ToDictionary(p => p.Key, p => p.Value);
            }
            return null;
        }
    }

    class Scanner
    {
        private readonly int ConnectionTimeout = 3000;
        private readonly int ReceiveTimeout = 2000;
        private readonly int SendTimeout = 2000;
        private readonly byte[] StartSignal = { 0x2, 0x32, 0x31, 0x3 };
        private readonly byte[] StopSignal = { 0x2, 0x32, 0x32, 0x3 };

        private int id;
        private string name;
        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public string Ip { get; private set; }
        public int Port { get; private set; }
        public string Message { get; set; }
        public TaskStatus Status { get; set; }
        public Scanner(int id, string name, string ip, int port)
        {
            this.id = id;
            this.name = name;
            this.Ip = ip;
            this.Port = port;
            this.Status = TaskStatus.Created;
        }

        public string SendSignalAndGetBarcode()
        {
            return Send(new[] { StartSignal, StopSignal }, true);
        }

        private string Send(byte[][] buffers, bool withReceive, int interval = 500)
        {
            string result = string.Empty;
            //var taskResult = @Socket.BeginConnect(IpEndPoint, null, null);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   //  创建Socket
            try
            {
                socket.ReceiveTimeout = ReceiveTimeout;
                socket.SendTimeout = SendTimeout;

                //创建终结点EndPoint
                IPAddress ip = IPAddress.Parse(Ip);
                var ipEndPoint = new IPEndPoint(ip, Port);   //把ip和端口转化为IPEndPoint的实例

                var taskResult = socket.BeginConnect(ipEndPoint, null, null);
                bool success = taskResult.AsyncWaitHandle.WaitOne(ConnectionTimeout, true);
                if (success)
                {
                    socket.EndConnect(taskResult);
                }
                else
                {
                    throw new SocketException(10060); // Connection timed out.
                }
                foreach (var buffer in buffers)
                {
                    socket.Send(buffer);
                    Thread.Sleep(interval);
                }
                if (withReceive)
                {
                    byte[] recvBytes = new byte[1024];
                    int bytes;
                    Thread.Sleep(300);
                    bytes = socket.Receive(recvBytes, recvBytes.Length, 0);    //从服务器端接受返回信息
                    result = Encoding.ASCII.GetString(recvBytes, 1, bytes - 2);
                }
            }
            finally
            {
                socket.Close();
            }

            return result;
        }
    }

    class ScannerGroup
    {
        private int id;
        private string name;
        public int Id { get { return id; } }
        public string Name { get { return name; } }

        public List<Scanner> ScannerList { get; set; }
        public ScannerGroup(int id, string name)
        {
            this.name = name;
            this.id = id;
        }
    }

    enum ScannerGroupId
    {
        Default,
        InboundScannerGroup = 100,
        OutboundScannerGroup = 200
    }

    enum ScannerId
    {
        Default,
        Inbound01 = 100010,
        Inbound02 = 100020,
        Outbound01 = 200010,
        Outbound02 = 200020
    }
}
