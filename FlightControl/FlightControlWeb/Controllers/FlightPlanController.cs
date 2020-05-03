using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private Dictionary<string, FlightPlan> idToItem;

        // GET /api/FlightPlan/{id}.
        [HttpGet("{id}", Name = "GetItem")]
        public ActionResult<FlightPlan> GetItem(string id)
        {
            bool isOK = idToItem.TryGetValue(id, out FlightPlan flightPlan);
            if (!isOK)
            {
                // Status 404 - Not found.
                return NotFound(id);
            }
            // Status 200 - found.
            return Ok(flightPlan);
        }

        // POST /api/FlightPlan.
        [HttpPost]
        public ActionResult Post([FromBody] FlightPlan flightPlan)
        {
            flightPlan.Id = IdAlgorithm();
            idToItem[flightPlan.Id] = flightPlan;
            // Status 201 - created.
            return CreatedAtAction(actionName: "GetItem", new { id = flightPlan.Id }, flightPlan);
        }

        private string IdAlgorithm()
        {
            return "shiraz318";
        }
    }


}