using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestApiVersionWithSwaggerWebApplication.V3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("3.0")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("3.0")]
        public string[] GetVersionV3([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new[] { Assembly.GetExecutingAssembly().FullName, memberName, sourceFilePath, sourceLineNumber.ToString() };
        }


        //[HttpGet]
        //[ApiVersionNeutral]
        //public string[] GetVersionNone([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    return new[] { Assembly.GetExecutingAssembly().FullName, memberName, sourceFilePath, sourceLineNumber.ToString() };
        //}

        //[HttpGet]
        //[ApiVersionNeutral]
        //public string[] GetVersion([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    return new[] { Assembly.GetExecutingAssembly().FullName, memberName, sourceFilePath, sourceLineNumber.ToString() };
        //}
    }
}