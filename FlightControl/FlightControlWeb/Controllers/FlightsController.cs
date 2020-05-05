using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/Flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        IFlightsManager manager = new FlightsManager();

        // DELETE /api/Flights/{id}.
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (manager.DeleteFlight(id))
            {
                return Ok();
            }
            return NotFound(id);
        }

        // Get /api/Flights?relative_to=<DATE_TIME>&sync_all
        [HttpGet]
        public ActionResult<Flights> GetAll([FromQuery] string relative_to, [FromQuery] string sync_all)
        {
            string dataTime = relative_to;
            string syncAll = sync_all;
            List<Flights> flights = new List<Flights>();
            return Ok(flights);
        }

        //// Get /api/Flights?relative_to=<DATE_TIME>
        //[HttpGet]
        //public ActionResult<Flights> GetInternal([FromQuery] string relative_to)
        //{
        //    string dataTime = relative_to;   
        //    List<Flights> flights = new List<Flights>();
        //    return Ok(flights);
        //}

       
    }
}