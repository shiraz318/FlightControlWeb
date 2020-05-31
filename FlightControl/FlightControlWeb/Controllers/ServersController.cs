using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        IServersManager manager;

        public ServersController(IServersManager manager)
        {
            this.manager = manager;
        }

        // GET: api/Servers
        [HttpGet]
        public ActionResult<List<Server>> Get()
        {
            List<Server> servers = new List<Server>();
            servers =  manager.Get();
            return Ok(servers);
        }

        // GET: api/Servers/{id}
        [HttpGet("{id}")]
        public ActionResult<Server> GetUrl(string id)
        {
            Server server = manager.GetServerByIdOfFlight(id);

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
        public ActionResult<string> Delete(string id)
        {
           if (manager.Delete(id))
            {
                return Ok(id);
            }
            return NotFound(id);
        }
    }
}
