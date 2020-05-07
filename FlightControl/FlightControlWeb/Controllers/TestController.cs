//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace FlightControlWeb.Controllers
//{
//    [Route("api/test")]
//    [ApiController]
//    public class TestController : ControllerBase
//    {
//        [HttpGet]
//        public IActionResult Get()
//        {
//            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
//            List<string> data = s.GetData();
//            if (data.Count < 1)
//            {
//                return NotFound();
//            }
//            string id = data[data.Count - 3];
//            string text_entry = data[data.Count - 2];
//            string price = data[data.Count - 1];
//            return Ok(new { id = id, text_entry= text_entry, price=price, c=data.Count });
//        }

//        [HttpPost("{data}/{num}")]
//        public IActionResult Post(string data, int num)
//        {
//            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
//            s.InsertData(data, num);
//            return Ok();
//        }
//    }
//}