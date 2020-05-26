using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb;

namespace UnitTestFlightControl
{
    [TestClass]
    public class ServersControllerTest
    {
        [TestMethod]
        public void GetServersTest()
        {
            // Arrange. 
            IDataAccess demoDataAccess = new DemoDataAccess();
            IServersManager serversManager = new ServersManager(demoDataAccess);
            ServersController serversController = new ServersController(serversManager);

            // Act.
            ActionResult<List<Server>> actionResult = serversController.Get();
            ObjectResult result = actionResult.Result as ObjectResult;
            List<Server> actualServers = result.Value as List<Server>;

            // Assert.
            List<Server> expectedServers = demoDataAccess.GetServers();
            // Get the minimum length.
            int size = actualServers.Count <= expectedServers.Count ? actualServers.Count : expectedServers.Count;
            int i = 0;
            for (i = 0; i < size; i++)
            {
                Assert.AreEqual<Server>(expectedServers[i], actualServers[i]);
            }
        }
    }
}
