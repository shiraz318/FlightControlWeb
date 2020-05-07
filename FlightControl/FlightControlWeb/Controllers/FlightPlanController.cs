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

        // GET /api/FlightPlan.
        [HttpGet]
        public FlightPlan[] GetAllFlightPlans()
        {
            FlightPlan[] flightPlans;
            flightPlans = manager.GetAllFlightPlans();
            return flightPlans;
        }

        // GET /api/FlightPlan/{id}.
        [HttpGet("{id}", Name = "GetItem")]
        public ActionResult GetItem(string id)
        {
            FlightPlan fp;
            if ((fp = manager.GetFlightPlan(id)) != null)
            {
                return Ok(fp);
            }
            return NotFound(id);
        }

        // POST /api/FlightPlan.
        [HttpPost]
        public ActionResult Post([FromBody] FlightPlan flightPlan)
        {
            string id = manager.AddFlightPlan(flightPlan);
            // Status 201 - created.
            //return CreatedAtAction(actionName: "GetItem", flightPlan);
            return CreatedAtAction(actionName: "GetItem", new { id = id }, flightPlan);
        }
    }
}