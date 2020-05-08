using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestApiVersionWithSwaggerWebApplication.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public string[] GetVersionV1([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new[] { Assembly.GetExecutingAssembly().FullName, memberName, sourceFilePath, sourceLineNumber.ToString() };
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public string[] GetVersionV2([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
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