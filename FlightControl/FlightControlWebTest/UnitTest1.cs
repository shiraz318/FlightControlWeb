using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightControlWebTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TestGetFlightPlanController()
        {
            string id = null;
            // Arrange
            
            // Fake.
            IDataAccess fakeDataAccess = new FakeDataAccess();
            IFlightPlanManager fakeFlightPlanManager = new FakeFlightPlanManager(fakeDataAccess);
            FlightPlanController fakeFlightPlanController = new FlightPlanController(fakeFlightPlanManager);
            // Tested object.
            IDataAccess dataAccess = new DataAccess();
            IFlightPlanManager flightPlanManager = new FlightPlanManager(dataAccess);
            FlightPlanController flightPlanController = new FlightPlanController(flightPlanManager);

            // Post.
            FlightPlan flightPlan = FakeFlightPlanObject();
            string id1 = flightPlanController.Post(flightPlan).Value;
            // Set return value of DataAccess.

            // Act
            FlightPlan testesFlightPlan = flightPlanController.GetItem(id1).Value;

            // Assert
            Assert.AreEqual(testesFlightPlan,fakeFlightPlanController.GetItem(id1));
        }


        [TestMethod]
        public FlightPlan FakeFlightPlanObject()
        {
            FlightPlan flightPlan = new FlightPlan();



            return flightPlan;

        }
    }
}
