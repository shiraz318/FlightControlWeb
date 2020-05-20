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
        IFlightPlanManager manager = new FlightPlanManager();

        // GET /api/FlightPlan?id={id}&url={url}.
        [HttpGet]
        public async Task<ActionResult<FlightPlan>> GetFlightPlanFromServer([FromQuery]string id, [FromQuery] string url)
        {
            try
            {
                FlightPlan fp;
                if ((fp = await manager.GetFlightPlanFromServer(id, url)) != null)
                {
                    return Ok(fp);
                }
                return NotFound(id);
            } catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // GET /api/FlightPlan/{id}/.
        [HttpGet("{id}", Name = "GetItem")]
        public async Task<ActionResult> GetItem(string id)
        {
            try
            {
                FlightPlan fp;
                if ((fp = await manager.GetFlightPlan(id)) != null)
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
        public async Task<ActionResult> Post([FromBody] FlightPlan flightPlan)
        {
            try
            {
                string id = await manager.AddFlightPlan(flightPlan);
                // Status 201 - created.
                //return CreatedAtAction(actionName: "GetItem", flightPlan);
                return CreatedAtAction(actionName: "GetItem", new { id = id }, flightPlan);
            }catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }
        
    }
}