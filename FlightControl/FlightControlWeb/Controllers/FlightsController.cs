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

        // Get /api/Flights?relative_to=<DATE_TIME>&sync_all or /api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public ActionResult<Flights> Get([FromQuery] string relative_to)
        {
            string dataTime = relative_to;
            string s = Request.QueryString.Value;
            List<Flights> flights = new List<Flights>();
            if (s.Contains("sync_all"))
            {
               flights = manager.GetAllFlights(dataTime, true);
            } 
            else
            {
               flights = manager.GetAllFlights(dataTime, false);
            }

            return Ok(flights);
        } 
    }
}