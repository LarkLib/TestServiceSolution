using Elite.Dsd;
using Sharp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;


namespace TestPLCConsoleApplication
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    // Create and connect the client
        //    var client = new S7Client();
        //    int result = client.ConnectTo("200.200.200.10", 0, 1);
        //    if (result == 0)
        //    {
        //        Console.WriteLine("200.200.200.10");
        //        byte[] buffer = new byte[38];
        //        //var readResult = client.ReadArea(S7Consts.S7AreaDB, 0, 0, 1, 0x02, buffer);
        //        var readResult = client.DBRead(7, 0, 6, buffer);
        //    }
        //    else
        //    {
        //        Console.WriteLine(client.ErrorText(result));
        //    }

        //    // Disconnect the client
        //    client.Disconnect();
        //}
        static void Main(string[] args)
        {
            //TestScanner();
            //Test2();
            //Test4();
            Test6();
            Test5();
        }

        private static void Test6()
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {

                // Output information about all of the installed voices.   
                Console.WriteLine("Installed voices -");
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    string AudioFormats = "";
                    foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                    {
                        AudioFormats += String.Format("{0}\n",
                        fmt.EncodingFormat.ToString());
                    }

                    Console.WriteLine(" Name:          " + info.Name);
                    Console.WriteLine(" Culture:       " + info.Culture);
                    Console.WriteLine(" Age:           " + info.Age);
                    Console.WriteLine(" Gender:        " + info.Gender);
                    Console.WriteLine(" Description:   " + info.Description);
                    Console.WriteLine(" ID:            " + info.Id);
                    Console.WriteLine(" Enabled:       " + voice.Enabled);
                    if (info.SupportedAudioFormats.Count != 0)
                    {
                        Console.WriteLine(" Audio formats: " + AudioFormats);
                    }
                    else
                    {
                        Console.WriteLine(" No supported audio formats found");
                    }

                    string AdditionalInfo = "";
                    foreach (string key in info.AdditionalInfo.Keys)
                    {
                        AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                    }

                    Console.WriteLine(" Additional Info - " + AdditionalInfo);
                    Console.WriteLine();
                }
            }
        }

        private static void Test5()
        {

            SpeechSynthesizer speech = new SpeechSynthesizer();

            speech.Rate = 0;//语速为正常语速
            speech.SelectVoice("Microsoft Lili");//设置播音员（中文）
            //speech.SelectVoice("Microsoft Anna"); //英文
            speech.Volume = 100;//音量为最大音量
            //speech.SpeakAsync("语音阅读方法");
            //speech.SpeakAsync("This is a testing string");
            //speech.Speak("This is a testing string,语音阅读方法");
            speech.Speak("1#堆垛机货物超宽");
        }

        private static void Test4()
        {
            var set = new HashSet<int>();
            for (int i = 0; i < 9; i++)
            {
                set.Add(100 + i);
            }
            //set.TryGetValue(); //4.7.2
            var hasItem = set.Contains(1);
            Console.WriteLine(set.First());
            Console.WriteLine(set.ToArray<int>()[0]);
            var enumerator = set.GetEnumerator();
            enumerator.MoveNext();
            Console.WriteLine(enumerator.Current);
        }

        private static void TestScanner()
        {
            var test = new ScannerOperation();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
                var result = test.GetScannerBarcodeByGroup((int)ScannerGroupId.InboundScannerGroup);
                foreach (var item in result)
                {
                    Console.WriteLine("result key:  {0} {1} {2}", item.Key.ToString(), "result value: ", item.Value == null ? "null" : item.Value.ToString());
                }
                Thread.Sleep(2000);
            }
            Console.ReadKey();
        }

        private static void Test2()
        {
            var test = new SyncInterface();
            test.TEConnectionString = "Data Source=.;Initial Catalog=test;Persist Security Info=True;User ID=taimao;Password=sd.258963;Asynchronous Processing=true";
            test.WmsConnectionString = "Data Source=.;Initial Catalog=dsdwms;Persist Security Info=True;User ID=taimao;Password=sd.258963;Asynchronous Processing=true";
            test.SyncDataFromETToWms(14001, "11");
            test.SyncDataFromETToWms(14002, "12");
        }

        static void Test3()
        {
            Say();　　　　　　　　　　　　　　　　　　　　　　　　　　　　　//由于Main不能使用async标记
            Console.ReadLine();
        }
        private async static void Say()
        {
            var t = TestAsync();
            //Thread.Sleep(1100);                                     //主线程在执行一些任务
            Console.WriteLine("Main Thread");                       //主线程完成标记
            Console.WriteLine(await t);                             //await 主线程等待取异步返回结果
        }
        static async Task<string> TestAsync()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(1000);                                 //异步执行一些任务
                return "Hello World";                               //异步执行完成标记
            });
        }
    }
}
