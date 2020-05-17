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
        public async Task<IEnumerable<Server>> Get()
        {
            List<Server> servers = new List<Server>();
            return await manager.Get();
        }
        // GET: api/Servers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Server>> GetUrl(string id)
        {
            Server server = await manager.GetServerByIdOfFlight(id);
            if (server != null)
            {
                return Ok(server);
            }
            return NotFound(id);
           
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
