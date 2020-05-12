using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/servers")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        IServersManager manager = new ServersManager();

        // GET: api/Servers
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<Server> servers = new List<Server>();
            manager.Get();
            return new string[] { "value1", "value2" };
        }

       
        // POST: api/Servers
        [HttpPost]
        public void Post([FromBody] Server server)
        {
            manager.Post(server);
        }

  
        // DELETE: api/servers/{id}
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            manager.Delete(id);
        }
    }
}
