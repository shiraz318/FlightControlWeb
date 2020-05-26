using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace FlightControlWeb.Controllers
{
    [Route("api/FlightPlan")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        IFlightPlanManager manager;

        public FlightPlanController(IFlightPlanManager manager)
        {
            this.manager = manager;
        }

        // GET /api/FlightPlan?id={id}&url={url}.
        [HttpGet]
        public async Task<ActionResult<FlightPlan>> GetFlightPlanFromServer([FromQuery]string id,
            [FromQuery] string url)
        {
            FlightPlan fp = await manager.GetFlightPlanFromServer(id, url);
            if (fp != null)
            {
                return Ok(fp);
            }
            else
            {
                return NotFound(id);
            }
        }

        // GET /api/FlightPlan/{id}/.
        [HttpGet("{id}", Name = "GetItem")]
        public async Task<ActionResult<FlightPlan>> GetItem(string id)
        {
            try
            {
                FlightPlan fp = await manager.GetFlightPlan(id);
                if (fp != null)
                {
                    return Ok(fp);
                }
                return NotFound(id);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
            
        // POST /api/FlightPlan.
        [HttpPost]
        public ActionResult<string> Post([FromBody] FlightPlan flightPlan)
        {
            string id =  manager.AddFlightPlan(flightPlan);
            return Ok(id);
        }
        
    }
}