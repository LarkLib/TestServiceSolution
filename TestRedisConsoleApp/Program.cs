using System;
using System.IO;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace TestRedisConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var connString = configuration.GetConnectionString("RedisConnectionString");

            string port = configuration.GetSection("AppSettings")["Port"];

            // redis-server --protected-mode no
            using (ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(connString))
            {
                var ret = redisConnection.GetDatabase().StringSet("name001", "value001", new TimeSpan(0, 0, 3));
                var value1 = redisConnection.GetDatabase().StringGet("name001");
                Console.WriteLine(value1);
                Thread.Sleep(5000);
                var value2 = redisConnection.GetDatabase().StringGet("name001");
                Console.WriteLine(value2);
            }


            //创建连接 建立订阅客户端
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connString))
            {
                ISubscriber sub = redis.GetSubscriber();

                //订阅名为 messages 的通道
                sub.Subscribe("messages", (channel, message) =>
                {

                    //输出收到的消息
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
                });
                Console.WriteLine("已订阅 messages");
                Console.ReadKey();
            }


        }
    }
}
