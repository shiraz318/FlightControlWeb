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
        private Dictionary<string, Flights> idToItem;

        // DELETE /api/Flights/{id}.
        [HttpDelete("{id}")]
        public ActionResult Delete([FromBody] string id)
        {
            Flights flight = idToItem[id];
            if (idToItem.Remove(id))
            {
                // Status 200 - id was deleted
                return Ok(flight);
            }
            // Status 404 - Not found.
            return NotFound(id);
        }
    }
}