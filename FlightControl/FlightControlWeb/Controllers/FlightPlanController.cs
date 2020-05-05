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

        private Dictionary<string, FlightPlan> idToItem = new Dictionary<string, FlightPlan>();
       
        public FlightPlanController()
        {
            InsertToDictionary();
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
            //flightPlan.Id = IdAlgorithm();
            //idToItem[flightPlan.Id] = flightPlan;
            
            manager.AddFlightPlan(flightPlan);
            //manager.AddFlightPlan(new FlightPlan("2"));
            // Status 201 - created.
            return CreatedAtAction(actionName: "GetItem", new { id = flightPlan.Id }, flightPlan);
        }

        private string IdAlgorithm()
        {
            return "shiraz318";
        }

        private void InsertToDictionary()
        {
            FlightPlan fp = new FlightPlan("1");
            idToItem.Add("1", fp);
        }
    }


}