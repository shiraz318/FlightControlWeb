using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static FlightControlWeb.Models.FlightsManager;

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
            try
            {
                if (manager.DeleteFlight(id))
                {
                    return Ok();
                }
                return NotFound(id);
            }catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // example /api/Flights?relative_to=<2020-05-06T10:12:00/Z>&sync_all
        // Get /api/Flights?relative_to=<DATE_TIME>&sync_all or /api/Flights?relative_to=<DATE_TIME>
        [HttpGet]

        public async Task<ActionResult<Flights>> Get([FromQuery] string relative_to)
        {
            try
            {
                string dateTime = relative_to;
                string s = Request.QueryString.Value;
                List<Flights> flights = new List<Flights>();
                FlightsFromServers flightsFromServers = new FlightsFromServers(flights, false);
                if (s.Contains("sync_all"))
                {
                    flightsFromServers =  await manager.GetAllFlights(dateTime, true);
                }
                else
                {
                    flightsFromServers = await manager.GetAllFlights(dateTime, false);
                }
                // At least one server did not responed.
                if (flightsFromServers.IsError)
                {
                    
                }
                return Ok(flightsFromServers.FlightsList);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}