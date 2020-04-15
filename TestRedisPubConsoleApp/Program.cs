using System;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TestRedisPubConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var connString = configuration.GetConnectionString("RedisConnectionString");

            Console.WriteLine("Hello World!");
            //创建连接 建立发布客户端
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connString))
            {
                ISubscriber sub = redis.GetSubscriber();

                Console.WriteLine("请输入任意字符，输入exit退出");

                string input;

                do
                {
                    input = Console.ReadLine();
                    sub.Publish("messages", input);
                } while (input != "exit");
            }

        }
    }
}
