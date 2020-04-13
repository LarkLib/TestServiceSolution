using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TetJWTConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestJWTClient();
            //TestConfigurationFile();
            TestConfigurationFileHostAsync();
        }
        private static async Task TestJWTClient()
        {
            // Console.WriteLine("Hello World!");
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:33800");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "aju",
                Scope = "api1"
            });
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
            //call api

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetAsync("http://localhost:33800/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            Console.ReadLine();
        }

        private static void TestConfigurationFile()
        {
            //https://www.cnblogs.com/stulzq/p/8570496.html
            //https://blog.elmah.io/config-transformations-in-aspnetcore/

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                //.AddJsonFile("appsettings.Debug.json");
                .AddJsonFile("appsettings.Debug.json", true, reloadOnChange: true);//reloadOnChange为true，文件发生更改时，就会重新载入配置到内存中来


            var config = builder.Build();
            //读取配置
            Console.WriteLine(config["Alipay:AppId"]);
            Console.WriteLine(config["Alipay:PriviteKey"]);

            //所有文件里的值
            foreach (var provider in config.Providers)
            {
                provider.TryGet("Alipay:AppId", out string val);

                Console.WriteLine(val);
            }

            //配置重载
            //reloadOnChange为true，文件发生更改时，就会重新载入配置到内存中来
            Console.WriteLine("更改文件之后，按下任意键");
            Console.ReadKey();

            Console.WriteLine("change:");
            Console.WriteLine(config["Alipay:AppId"]);
            Console.WriteLine(config["Alipay:PriviteKey"]);

            Console.ReadKey();

            return;
        }
        private static async Task TestConfigurationFileHostAsync()
        {
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1
            //https://garywoodfine.com/ihost-net-core-console-applications/
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-3.1
            //https://www.cnblogs.com/tdfblog/p/Environments-LaunchSettings-in-Asp-Net-Core.html

            const string _prefix = "FUZZBIZZ_";
            const string _appsettings = "appsettings.json";
            const string _hostsettings = "hostsettings.json";
            var host = new HostBuilder()
                                .ConfigureHostConfiguration(configHost =>
                                {
                                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                                    configHost.AddJsonFile(_hostsettings, optional: true);
                                    configHost.AddEnvironmentVariables(prefix: _prefix);
                                    //configHost.AddCommandLine(args);
                                })
                                .ConfigureAppConfiguration((hostContext, configApp) =>
                                {
                                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                                    configApp.AddJsonFile(_appsettings, optional: true);
                                    configApp.AddJsonFile(
                                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",//Set the ASPNETCORE_ENVIRONMENT environmental variable.
                                        optional: true);
                                    configApp.AddEnvironmentVariables(prefix: _prefix);
                                    //configApp.AddCommandLine(args);
                                })
                                .ConfigureServices((hostContext, services) =>
                                {
                                    //services.AddLogging();
                                    //services.Configure<Application>(hostContext.Configuration.GetSection("application"));
                                    //services.AddHostedService<FizzBuzzHostedService>();
                                })
                                .ConfigureLogging((hostContext, configLogging) =>
                                {
                                    //configLogging.AddConsole();
                                })
                                .UseConsoleLifetime()
                                .Build();

            await host.RunAsync();
            Console.ReadKey();

            return;
        }
    }
}