using System;
using System.Configuration;
using System.Web.Http;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using System.Web.Cors;
using System.Web.Http.Cors;
using System.Web.Http.Hosting;

[assembly: OwinStartup(typeof(AGVManagement.Startup))]
namespace AGVManagement
{
    /// <summary>
    /// Owin必须的启动类，不要改名
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(ConfigureWebApi());
        }

        private HttpConfiguration ConfigureWebApi()
        {
            HttpConfiguration config = new HttpConfiguration();


            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new Newtonsoft.Json.Converters.IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd hh:mm:ss"
                });

            //允许通过属性显式设置的路由
            config.MapHttpAttributeRoutes();

            //允许未显式设置路由的API使用默认路由
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            //运行跨域
            config.EnableCors();

            return config;
        }
    }
}
