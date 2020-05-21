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
        public ActionResult<IEnumerable<Server>> Get()
        {
            try
            {
                List<Server> servers = new List<Server>();
                servers =  manager.Get();
                return Ok(servers);

            }catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // GET: api/Servers/{id}
        [HttpGet("{id}")]
        public ActionResult<Server> GetUrl(string id)
        {
            try
            {
                Server server = manager.GetServerByIdOfFlight(id);
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
        public ActionResult<bool> Delete(string id)
        {
           if (manager.Delete(id))
            {
                return Ok(id);
            }
            return NotFound(id);
        }
    }
}
