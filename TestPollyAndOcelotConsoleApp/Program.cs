using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using Polly;
using Polly.Caching;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;

namespace TestPollyExceptionPolicyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestPolly.TestRetry();
            TestOcelot();
        }
        public static void TestOcelot()
        {
            //https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html
            //open http://localhost:5000/todos/1 in browser
            new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddJsonFile("ocelot.json")
                    .AddEnvironmentVariables();
            })
            .ConfigureServices(s => {
                s.AddOcelot();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                   //add your logging
               })
            .UseIISIntegration()
            .Configure(app =>
            {
                app.UseOcelot().Wait();
            })
            .Build()
            .Run();
        }
    }

    class TestPolly
    {
        #region TestSomething
        //Polly 可以实现重试、断路、超时、隔离、回退和缓存策略
        public static void TestSomething()
        {
            //断路（Circuit-breaker）
            //这句代码设定的策略是，当系统出现两次某个异常时，就停下来，等待 1 分钟后再继续。这是基本的用法，你还可以在断路时定义中断的回调和重启的回调。
            Policy.Handle<SomeException>().CircuitBreaker(2, TimeSpan.FromMinutes(1));

            //超时（Timeout）
            //当系统超过一定时间的等待，我们就几乎可以判断不可能会有成功的结果。比如平时一个网络请求瞬间就完成了，如果有一次网络请求超过了 30 秒还没完成，我们就知道这次大概率是不会返回成功的结果了。因此，我们需要设置系统的超时时间，避免系统长时间做无谓的等待。
            //这里设置了超时时间不能超过 30 秒，否则就认为是错误的结果，并执行回调。
            Policy.Timeout(30, onTimeout: (context, timespan, task) =>
            {
                // do something
            });


            //隔离（Bulkhead Isolation）
            //当系统的一处出现故障时，可能促发多个失败的调用，很容易耗尽主机的资源(如 CPU)。
            //下游系统出现故障可能导致上游的故障的调用，甚至可能蔓延到导致系统崩溃。所以要将可控的操作限制在一个固定大小的资源池中，以隔离有潜在可能相互影响的操作。
            //下面是隔离策略的一个基本用法：
            //这个策略是最多允许 12 个线程并发执行，如果执行被拒绝，则执行回调。
            Policy.Bulkhead(12, context =>
            {
                // do something
            });

            //回退（Fallback）
            //有些错误无法避免，就要有备用的方案。这个就像浏览器不支持一些新的 CSS 特性就要额外引用一个 polyfill 一样。
            //一般情况，当无法避免的错误发生时，我们要有一个合理的返回来代替失败。
            //比如很常见的一个场景是，当用户没有上传头像时，我们就给他一个默认头像，这种策略可以这样定义：
            //Policy.Handle<SomeException>().Fallback<UserAvatar>(() => UserAvatar.GetRandomAvatar());
            Policy.Handle<SomeException>().Fallback(() => { Console.WriteLine("Fallback"); });

            //缓存（Cache）
            //一般我们会把频繁使用且不会怎么变化的资源缓存起来，以提高系统的响应速度。
            //如果不对缓存资源的调用进行封装，那么我们调用的时候就要先判断缓存中有没有这个资源，有的话就从缓存返回，否则就从资源存储的地方（比如数据库）获取后缓存起来，
            //再返回，而且有时还要考虑缓存过期和如何更新缓存的问题。Polly 提供了缓存策略的支持，使得问题变得简单。
            //这是官方的一个使用示例用法，它定义了缓存 5 分钟过期的策略，然后把这个策略应用在指定的 Key（即 FooKey）上。
            //这一块内容值得用一整篇的内容来讲，下次有机会再详细讲讲 Polly 的缓存策略。
            var memoryCacheProvider = new MemoryCacheProvider();
            var cachePolicy = Policy.Cache(memoryCacheProvider, TimeSpan.FromMinutes(5));
            var result = cachePolicy.Execute(context => getFoo(), new Context("FooKey"));

            //策略包（Policy Wrap）
            //一种操作会有多种不同的故障，而不同的故障处理需要不同的策略。这些不同的策略必须包在一起，作为一个策略包，才能应用在同一种操作上。这就是文章开头说的 Polly 的弹性，即各种不同的策略能够灵活地组合起来。
            //策略包的基本用法是这样的：
            //先是把预先定义好的多种不同的策略包在一起，作为一个整体策略，然后应用在同一个操作上。
            //var policyWrap = Policy.Wrap(fallback, cache, retry, breaker, timeout, bulkhead);
            //policyWrap.Execute();
        }

        private static string getFoo()
        {
            throw new NotImplementedException();
        }
        private class SomeException : Exception
        {
        }
        private class UserAvatar
        {
            public void GetRandomAvatar() { }
        }
        private class MemoryCacheProvider : ISyncCacheProvider
        {
            public void Put(string key, object value, Ttl ttl)
            {
                throw new NotImplementedException();
            }

            public (bool, object) TryGet(string key)
            {
                throw new NotImplementedException();
            }
        }
        #endregion TestSomething

        #region TestRetry
        public static void TestRetry()
        {
            Policy
                // 1. 指定要处理什么异常
                .Handle<HttpRequestException>()
                //    或者指定需要处理什么样的错误返回
                .OrResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.BadGateway)
                // 2. 指定重试次数和重试策略
                .Retry(3, (exception, retryCount, context) =>
                {
                    Console.WriteLine($"开始第 {retryCount} 次重试：");
                })
                // 3. 执行具体任务
                .Execute(ExecuteMockRequest);

            Console.WriteLine("程序结束，按任意键退出。");
            Console.ReadKey();
        }

        static HttpResponseMessage ExecuteMockRequest()
        {
            // 模拟网络请求
            Console.WriteLine("正在执行网络请求...");
            Thread.Sleep(3000);
            // 模拟网络错误
            return new HttpResponseMessage(HttpStatusCode.BadGateway);
        }
        #endregion TestRetry
    }
}
