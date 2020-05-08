using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestWebApiVersionWebApplication.Controllers
{
    //https://localhost:44381/api/Values
    //https://localhost:44381/api/Values?api-version=1.0
    //GET https://localhost:44381/api/Values HTTP/1.1 //invoke by request header
    //User-Agent: Fiddler
    //x-api-version: 2.0
    //Host: localhost:44381
    //[ApiVersionNeutral] //remove the version from the specific API
    [ApiVersion("1.0")]
    [Route("api/Values")]
    [ApiController]
    public class ValuesQueryStringV1Controller : ControllerBase
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "ValuesQueryStringV1Controller value one from api version", "One" };
        }
    }

    //https://localhost:44381/api/Values?api-version=2.0
    [ApiVersion("2.0")]
    [Route("api/Values")]
    [ApiController]
    public class ValuesQueryStringV2Controller : ControllerBase
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "ValuesQueryStringV2Controller value two from the api version", "two" };
        }
    }
}