using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace TestMiniProfilerWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();

            //https://www.cnblogs.com/lwqlun/p/10222505.html
            //默认的路径是/mini-profiler-resources
            //http://localhost:5000/mini-profiler-resources/results
            //https://localhost:44396/profiler/results
            services.AddMiniProfiler(options => options.RouteBasePath = "/profiler");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TestMiniProfilerWebApplication",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //test MiniProfiler
            app.UseMiniProfiler();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //Enable Static File Middleware: for custom swagger ui
            app.UseStaticFiles();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //默认的index.html页面可以从如下链接下载
            //https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/src/Swashbuckle.AspNetCore.SwaggerUI/index.html
            //下载之后将这个文件放置到项目根目录下。
            //接下来我们需要在这个文件的头部加入如下脚本代码：
            //Copy
            //< script async = "async" id = "mini-profiler" src = "/profiler/includes.min.js?v=4.0.138+gcc91adf599"
            //    data - version = "4.0.138+gcc91adf599" data - path = "/profiler/"
            //    data - current - id = "4ec7c742-49d4-4eaf-8281-3c1e0efa748a" data - ids = "" data - position = "Left"
            //    data - authorized = "true" data - max - traces = "15" data - toggle - shortcut = "Alt+P"
            //    data - trivial - milliseconds = "2.0" data - ignored - duplicate - execute - types = "Open,OpenAsync,Close,CloseAsync" >
            //</ script >
            //最后我们需要配置这个index.html文件的Bulid Action为Embedded resource
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("TestMiniProfilerWebApplication.index.html");
            });
        }
    }
}
