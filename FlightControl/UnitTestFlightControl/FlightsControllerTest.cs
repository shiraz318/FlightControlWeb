//using FlightControlWeb;
//using FlightControlWeb.Controllers;
//using FlightControlWeb.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Net.Http;
//using System.Text;

//namespace UnitTestFlightControl
//{
//    [TestClass]
//    public class FlightsControllerTest
//    {
//        [TestMethod]
//        public async System.Threading.Tasks.Task GetFlightsTestAsync()
//        {
//            // Arrange.
//            IDataAccess demoDataAccess = new DemoDataAccess();
//            IFlightsManager flightsManager = new FlightsManager(demoDataAccess);
//            FlightsController flightsController = new FlightsController(flightsManager);

//            // Act.
//            HttpRequest request = flightsController.Request;
//            try
//            {
//                QueryString q = new QueryString("relative_to=2020-05-05T05:05:05Z");
//                flightsController.Request.QueryString = q;
//            }
//            catch (Exception e)
//            {
//                string message = e.Message;
//            }
//            //var queryString = new NameValueCollection { { "relative_to", "2020-05-05T05:05:05Z" } };
//            //request.Query = "";
//            //request.QueryString["code"]
//            //flightsController.Request.Query.
//            //flightsController.Request = new HttpRequestMessage
//            //{
//            //    RequestUri = new Uri("http://localhost/api/products")
//            //};

//            // api/Flights?relative_to=<DATE_TIME>&sync_all or
//            ActionResult<List<Flights>> actionResult = await flightsController.Get("2020-05-05T05:05:05Z");
//            ObjectResult result = actionResult.Result as ObjectResult;
//            List<Flights> actualFlights = result.Value as List<Flights>;

//            // Assert.
//            DateTime time = Convert.ToDateTime("2020-05-05T05:05:05Z");
//            List<Flights> expectedFlights = demoDataAccess.GetFlights(time);
//            int size = actualFlights.Count <= expectedFlights.Count ? actualFlights.Count : expectedFlights.Count;
//            int i = 0;
//            for (i = 0; i < size; i++)
//            {
//                Assert.AreEqual<Flights>(actualFlights[i], expectedFlights[i]);
//            }

//            //FlightPlan expectedFlightPlan = demoDataAccess.GetFlightPlan(id);
//            // Assert.AreEqual<FlightPlan>(actualFlightPlan, expectedFlightPlan);
//        }
//    }
//}
