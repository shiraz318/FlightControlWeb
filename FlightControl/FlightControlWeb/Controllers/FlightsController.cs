using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private IFlightsManager manager;

        public FlightsController(IFlightsManager manager)
        {
            this.manager = manager;
        }

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

        // Get /api/Flights?relative_to=<DATE_TIME>&sync_all or
        // /api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public async Task<ActionResult<List<Flights>>> Get([FromQuery] string relative_to)
        {
            try
            {
                string urlRequest = Request.QueryString.Value;
                List<Flights> flights = new List<Flights>();
                FlightsFromServers flightsFromServers = new FlightsFromServers(flights, false);
                if (urlRequest.Contains("sync_all"))
                {
                    flightsFromServers =  await manager.GetAllFlights(relative_to, true);
                }
                else
                {
                    flightsFromServers = await manager.GetAllFlights(relative_to, false);
                }
               
                // At least one response in not valid.
                if (flightsFromServers.IsError)
                {
                    return BadRequest();
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