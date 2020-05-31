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
        public const string Valid = "Ok";
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
        private string CheckValidationOfFlightPlan(FlightPlan flightPlan)
        {
            if (flightPlan.InitialLocation.Longitude > LongitudeBorder
                || flightPlan.InitialLocation.Longitude < -LongitudeBorder)
            {
                return "Oops! Something Is Wrong. Flight Plan Initial Location Is Invalid";
            }
            if (flightPlan.InitialLocation.Latitude > LatitudeBorder 
                || flightPlan.InitialLocation.Latitude < -LatitudeBorder)
            {
                return "Oops! Something Is Wrong. Flight Plan Initial Location Is Invalid";
            }
            if (flightPlan.Passengers < 0)
            {
                return "Oops! Something Is Wrong. Flight Plan Passenger's Number Is Invalid";
            }
            if (flightPlan.CompanyName == null)
            {
                return "Oops! Something Is Wrong. Flight Plan Company Name Is Invalid";
            }
            if (flightPlan.Segments.Count == 0)
            {
                return "Oops! Something Is Wrong. Flight Plan Segments Is Invalid";
            }
            foreach (Segment segment in flightPlan.Segments)
            {
                if (segment.Longitude > LongitudeBorder || segment.Longitude < -LongitudeBorder)
                {
                    return "Oops! Something Is Wrong. Flight Plan Segment Location Is Invalid";
                }
                if (segment.Latitude > LatitudeBorder || segment.Latitude < -LatitudeBorder)
                {
                    return "Oops! Something Is Wrong. Flight Plan Segment Location Is Invalid";
                }
                if (segment.TimespanSeconds < 0)
                {
                    return "Oops! Something Is Wrong. Flight Plan Segment Timespan Seconds Is Invalid";
                }
            }
            return Valid;
        }

        // POST /api/FlightPlan.
        [HttpPost]
        public ActionResult<string> Post([FromBody] FlightPlan flightPlan)
        {
            string isValid = CheckValidationOfFlightPlan(flightPlan);
            if (isValid.CompareTo(Valid) != 0)
            {
                return BadRequest(isValid);
            }
            string id =  manager.AddFlightPlan(flightPlan);
            return Ok(id);
        }
        
    }
}