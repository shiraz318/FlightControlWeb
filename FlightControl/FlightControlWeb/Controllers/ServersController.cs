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
        public async Task<ActionResult<IEnumerable<Server>>> Get()
        {
            try
            {
                List<Server> servers = new List<Server>();
                servers = await manager.Get();
                return Ok(servers);

            }catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // GET: api/Servers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Server>> GetUrl(string id)
        {
            try
            {
                Server server = await manager.GetServerByIdOfFlight(id);
                if (server != null)
                {
                    return Ok(server);
                }
                return NotFound(id);
            } catch(Exception e)
            {
                return NotFound(e.Message);
            }
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
