using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestLector620ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSocket2(args);
            //TestSocket();
            //TestTcpClient();
        }
        private static void TestSocket()
        {
            try
            {
                int port = 2112;
                string host = "200.200.200.68";
                //创建终结点EndPoint
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);   //把ip和端口转化为IPEndPoint的实例

                //创建Socket并连接到服务器
                Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   //  创建Socket
                Console.WriteLine("Connecting...");
                c.Connect(ipe); //连接到服务器

                //向服务器发送信息
                //string sendStr = "Hello,this is a socket test";
                byte[] bs = { 0x2, 0x32, 0x31, 0x3 };// Encoding.ASCII.GetBytes(sendStr);   //把字符串编码为字节
                Console.WriteLine("Send message");
                c.Send(bs, bs.Length, 0); //发送信息
                Thread.Sleep(2000);
                c.Send(new byte[] { 0x2, 0x32, 0x32, 0x3 }, 4, 0); //发送信息
                //接受从服务器返回的信息
                string recvStr = "";
                byte[] recvBytes = new byte[1024];
                int bytes;
                bytes = c.Receive(recvBytes, recvBytes.Length, 0);    //从服务器端接受返回信息
                recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
                Console.WriteLine("client get message:{0}", recvStr);    //回显服务器的返回信息

                //一定记着用完Socket后要关闭
                c.Close();
                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("argumentNullException:{0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException:{0}", e);
            }
        }
        private static void TestSocket2(string[] args)
        {
            try
            {
                var arg = args == null || args.Length < 1 ? "21" : args[0];
                int port = 2112;
                string host = "200.200.200.68";
                //创建终结点EndPoint
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);   //把ip和端口转化为IPEndPoint的实例

                //创建Socket并连接到服务器
                Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   //  创建Socket
                Console.WriteLine("Connecting...");
                c.Connect(ipe); //连接到服务器

                if (arg == "21")
                {
                    //向服务器发送信息
                    //string sendStr = "Hello,this is a socket test";
                    byte[] bs = { 0x2, 0x32, 0x31, 0x3 };// Encoding.ASCII.GetBytes(sendStr);   //把字符串编码为字节
                    Console.WriteLine("Send message");
                    c.Send(bs, bs.Length, 0); //发送信息
                }
                else
                {
                    //Thread.Sleep(2000);
                    c.Send(new byte[] { 0x2, 0x32, 0x32, 0x3 }, 4, 0); //发送信息
                    //接受从服务器返回的信息
                    string recvStr = "";
                    byte[] recvBytes = new byte[1024];
                    int bytes;
                    bytes = c.Receive(recvBytes, recvBytes.Length, 0);    //从服务器端接受返回信息
                    recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
                    Console.WriteLine("client get message:{0}", recvStr);    //回显服务器的返回信息
                }

                //一定记着用完Socket后要关闭
                c.Close();
                Console.ReadLine();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("argumentNullException:{0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException:{0}", e);
            }
        }

        private static void TestTcpClient()
        {
            Console.WriteLine("客户端启动……");
            using (TcpClient client = new TcpClient())
            {

                //与服务器连接
                client.Connect("200.200.200.68", 2112);

                // 打印连接到的服务端信息
                Console.WriteLine("已经成功与客户端建立连接！{0} --> {1}", client.Client.LocalEndPoint, client.Client.RemoteEndPoint);

                NetworkStream stream = client.GetStream();//创建于服务器连接的数据流
                //发送字符串

                byte[] startScanCommand = { 0x2, 0x32, 0x31, 0x3 }; //扫描开始命令
                stream.Write(startScanCommand, 0, startScanCommand.Length);
                Console.WriteLine("发送扫描开始命令");
                Thread.Sleep(2000);
                byte[] endscanCommand = { 0x2, 0x32, 0x32, 0x3 }; //扫描结束命令
                stream.Write(endscanCommand, 0, startScanCommand.Length);
                Console.WriteLine("发送扫描结束命令");

                //接收字符串
                Thread.Sleep(500);
                byte[] result = new byte[client.Available]; // tcp.Available为接受的字符串大小
                //byte[] result = new byte[1024]; // tcp.Available为接受的字符串大小
                stream.Read(result, 0, result.Length);//接受服务器回报的字符串
                stream.Close();
                string strResponse = Encoding.ASCII.GetString(result).Trim();//从服务器接受到的字符串
                Console.WriteLine(strResponse);
                Console.ReadKey();
            }
        }
        //don't working now
        private async static void TestTcpClientAsync()
        {
            Console.WriteLine("客户端启动……");
            using (TcpClient client = new TcpClient())
            {

                //与服务器连接
                client.Connect("200.200.200.68", 2112);

                // 打印连接到的服务端信息
                Console.WriteLine("已经成功与客户端建立连接！{0} --> {1}", client.Client.LocalEndPoint, client.Client.RemoteEndPoint);


                NetworkStream stream = client.GetStream();//创建于服务器连接的数据流
                //发送字符串

                byte[] startScanCommand = { 0x2, 0x32, 0x31, 0x3 }; //扫描开始命令
                await stream.WriteAsync(startScanCommand, 0, startScanCommand.Length);
                Console.WriteLine("发送扫描开始命令");
                Thread.Sleep(2000);
                byte[] endscanCommand = { 0x2, 0x32, 0x32, 0x3 }; //扫描结束命令
                await stream.WriteAsync(endscanCommand, 0, startScanCommand.Length);
                Console.WriteLine("发送扫描结束命令");

                //接收字符串
                //Thread.Sleep(500);
                byte[] result = new byte[client.Available]; // tcp.Available为接受的字符串大小
                //byte[] result = new byte[1024]; // tcp.Available为接受的字符串大小
                var ret = await stream.ReadAsync(result, 0, result.Length);//接受服务器回报的字符串
                stream.Close();
                string strResponse = Encoding.ASCII.GetString(result).Trim();//从服务器接受到的字符串
                Console.WriteLine(strResponse);
                Console.ReadKey();
            }
        }
    }
}
