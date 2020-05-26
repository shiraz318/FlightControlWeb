using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestFlightControl
{
    [TestClass]
    public class FlightPlanControllerTest
    {
       
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
        public void OperateTest(string id)
        {
            GetFlightPlanTest(id);
        }
    }
}


