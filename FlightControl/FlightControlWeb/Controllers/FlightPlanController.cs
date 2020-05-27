using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static FlightControlWeb.Models.FlightPlan;

namespace FlightControlWeb.Controllers
{
    [Route("api/FlightPlan")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        public const int LongitudeBorder = 180;
        public const int LatitudeBorder = 90;
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

        // Check the validation of a given flightPlan.
        private bool CheckValidationOfFlightPlan(FlightPlan flightPlan)
        {
            if (flightPlan.InitialLocation.Longitude > LongitudeBorder
                || flightPlan.InitialLocation.Longitude < -LongitudeBorder)
            {
                return false;
            }
            if (flightPlan.InitialLocation.Latitude > LatitudeBorder 
                || flightPlan.InitialLocation.Latitude < -LatitudeBorder)
            {
                return false;
            }
            if (flightPlan.Passengers < 0)
            {
                return false;
            }
            if (flightPlan.CompanyName == null)
            {
                return false;
            }
            if (flightPlan.Segments.Count == 0)
            {
                return false;
            }
            foreach (Segment segment in flightPlan.Segments)
            {
                if (segment.Longitude > LongitudeBorder || segment.Longitude < -LongitudeBorder)
                {
                    return false;
                }
                if (segment.Latitude > LatitudeBorder || segment.Latitude < -LatitudeBorder)
                {
                    return false;
                }
                if (segment.TimespanSeconds < 0)
                {
                    return false;
                }
            }
            return true;
        }

        // POST /api/FlightPlan.
        [HttpPost]
        public ActionResult<string> Post([FromBody] FlightPlan flightPlan)
        {
            if (!CheckValidationOfFlightPlan(flightPlan))
            {
                return BadRequest("");
            }
            string id =  manager.AddFlightPlan(flightPlan);
            return Ok(id);
        }
        
    }
}