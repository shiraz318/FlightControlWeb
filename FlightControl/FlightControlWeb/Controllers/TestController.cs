using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
            List<string> data = s.GetData();
            if (data.Count < 1)
            {
                return NotFound();
            }
            string d1 = data[data.Count - 1];
            return Ok(new { name = d1, c=data.Count });
        }

        [HttpPost("{data}/{num}")]
        public IActionResult Post(string data, int num)
        {
            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
            s.InsertData(data, num);
            return Ok();
        }
    }
}