using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestWebApiVersionWebApplication.Controllers
{
    //https://localhost:44381/api/v1.0/Values
    //[ApiVersionNeutral] //remove the version from the specific API
    [ApiVersion("1.0", Deprecated = true)]//deprecate the specfic API version
    [Route("api/v{v:apiVersion}/Values")]
    [ApiController]
    public class ValuesV1Controller : ControllerBase
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "ValuesV1Controller value one from api version", "One" };
        }
    }

    //https://localhost:44381/api/v2.0/Values
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/Values")]
    [ApiController]
    public class ValuesV2Controller : ControllerBase
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "ValuesV2Controller value two from the api version", "two" };
        }
    }
}