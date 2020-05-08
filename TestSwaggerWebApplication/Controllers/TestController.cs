using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestSwaggerWebApplication.Controllers
{
#pragma warning disable CS1591
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Get todo item
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        /// </remarks>
        /// <param name="name">a name from client</param>
        /// <returns>a todo item list </returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response> 
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IEnumerable<TodoItem> GetTodoItem(string name)
        {
            var item = new TodoItem() { Id = 1, Name = name, IsComplete = true };
            var item2 = new TodoItem() { Id = 2, Name = name, IsComplete = false };
            return new[] { item, item2 };
        }

        /// <summary>
        /// Say hello to world
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>hello world string</returns>
        [HttpGet("GetHelloWorld")]
        public string GetHelloWorld(string name)
        {
            return string.Format("{0} say: Hello, World", name);
        }
    }

    public class TodoItem
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool IsComplete { get; set; }
    }
#pragma warning restore CS1591
}