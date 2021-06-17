using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApiModulo4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //El Authorize es una etiqueta para darle seguridad a este controller
    //[Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ValuesController(IConfiguration configuration )
        {
            this.configuration = configuration;
        }

        // GET api/values
        [HttpGet]
        [ResponseCache(Duration = 15)]
        public ActionResult<string> Get()
        {
            return DateTime.Now.Second.ToString();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return configuration["apellido"];
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
