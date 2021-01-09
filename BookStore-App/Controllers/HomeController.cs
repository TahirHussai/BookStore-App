using BookStore_App.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: api/<HomeController>
        /// <summary>
        /// This is a test API Controller
        /// </summary>
        /// <returns></returns>
        private readonly ILoggerService _loggerService;
        public HomeController(ILoggerService loggerService)
        {
            _loggerService = loggerService;

        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _loggerService.LogInfo("get Logger Info");
            return new string[] { "value1", "value2" };
        }

        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _loggerService.LogInfo("get Logger Info");

            return "value";
        }

        // POST api/<HomeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _loggerService.LogDebug("get Logger Debuger");

        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _loggerService.LogDebug("get Logger Error");

        }
    }
}
