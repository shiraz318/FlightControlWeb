using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using static FlightControlWeb.Models.FlightPlan;

namespace UnitTestFlightControl
{
    [TestClass]
    public class FlightPlanControllerTest
    {
        private static Dictionary<FlightPlan, ObjectResult> CreateFlightPlans()
        {
            Dictionary<FlightPlan, ObjectResult> flightPlansElements =
                new Dictionary<FlightPlan, ObjectResult>();

            Location validLocation = new Location(10, 10, new DateTime());
            Location invalidLocation = new Location(200, 20, new DateTime());
            List<Segment> validSegments = new List<FlightPlan.Segment>();
            List<Segment> invalidSegments1 = new List<FlightPlan.Segment>();
            List<Segment> invalidSegments2 = new List<FlightPlan.Segment>();

            Segment validSegment = new Segment(10, 10, 10);
            Segment invalidSegment = new Segment(20, 20, -20);

            validSegments.Add(validSegment);
            invalidSegments2.Add(invalidSegment);

            FlightPlan validFlight = 
                new FlightPlan(10, "El-Al", validLocation, validSegments);
            FlightPlan invalidPassengersFlight =
                new FlightPlan(-200, "El-Al", validLocation, validSegments);
            FlightPlan invalidCompanyNameFlight =
                new FlightPlan(10, null, validLocation, validSegments);
            FlightPlan invalidLocationFlight =
                new FlightPlan(10, "El-Al", invalidLocation, validSegments);
            FlightPlan invalidSegment1Flight =
                new FlightPlan(10, "El-Al", validLocation, invalidSegments1);
            FlightPlan invalidSegment2Flight =
                new FlightPlan(10, "El-Al", validLocation, invalidSegments2);

            flightPlansElements.Add(validFlight, new OkObjectResult(""));
            flightPlansElements.Add(invalidPassengersFlight, new BadRequestObjectResult(""));
            flightPlansElements.Add(invalidCompanyNameFlight, new BadRequestObjectResult(""));
            flightPlansElements.Add(invalidLocationFlight, new BadRequestObjectResult(""));
            flightPlansElements.Add(invalidSegment1Flight, new BadRequestObjectResult(""));
            flightPlansElements.Add(invalidSegment2Flight, new BadRequestObjectResult(""));

            return flightPlansElements;

        }

        static IEnumerable<object[]> FlightPlanTestDataProperty
        {
             get
            {
                Dictionary<FlightPlan, ObjectResult> dictionary = CreateFlightPlans();
                List<object[]> list = new List<object[]>();
                foreach(KeyValuePair<FlightPlan, ObjectResult> element in dictionary)
                {
                    list.Add(new object[] { element.Key, element.Value});
                }

                return list;
            }
        }
        public async void GetFlightPlanTest(string id)
        {
            // Arrange.
            IDataAccess demoDataAccess = new DemoDataAccess();
            IFlightPlanManager flightPlanManager = new FlightPlanManager(demoDataAccess);
            FlightPlanController flightPlanController = new FlightPlanController(flightPlanManager);

            // Act.
            ActionResult<FlightPlan> actionResult = await flightPlanController.GetItem(id);
            ObjectResult result = actionResult.Result as ObjectResult;
            FlightPlan actualFlightPlan = result.Value as FlightPlan;

            // Assert.
            FlightPlan expectedFlightPlan = demoDataAccess.GetFlightPlan(id);
            Assert.AreEqual<FlightPlan>(actualFlightPlan, expectedFlightPlan);
        }

        [TestMethod]
        [DataRow("exists")]
        [DataRow("not_exists")]
        public void OperateGetFlightPlanTest(string id)
        {
            GetFlightPlanTest(id);
        }


        [TestMethod]
        [DynamicData("FlightPlanTestDataProperty")]
        public void PostFlghtPlanTest(FlightPlan flightPlan, ObjectResult expectedResult)
        {
            // Arrange.
            IDataAccess demoDataAccess = new DemoDataAccess();
            IFlightPlanManager flightPlanManager = new FlightPlanManager(demoDataAccess);
            FlightPlanController flightPlanController =
                new FlightPlanController(flightPlanManager);

            // Act.
            ActionResult<string> actionResult = flightPlanController.Post(flightPlan);

            // Assert.
            Assert.IsInstanceOfType(actionResult.Result, expectedResult.GetType());
        }
    }
}


