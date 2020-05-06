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

        // example /api/Flights?relative_to=<2020-05-06T10:12:00Z>&sync_all
        // Get /api/Flights?relative_to=<DATE_TIME>&sync_all or /api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public ActionResult<Flights> Get([FromQuery] string relative_to)
        {
            string dateTime = relative_to;
            string s = Request.QueryString.Value;
            List<Flights> flights = new List<Flights>();
            if (s.Contains("sync_all"))
            {
               flights = manager.GetAllFlights(dateTime, true);
            } 
            else
            {
               flights = manager.GetAllFlights(dateTime, false);
            }

            return Ok(flights);
        } 
    }
}