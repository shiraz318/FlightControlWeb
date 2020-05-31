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

        [TestMethod]
        [DataRow("exists_serverID")]
        [DataRow("not_exists_serverID")]
        public void DeleteServerTest(string id)
        {
            // Arrange.
            IDataAccess demoDataAccess = new DemoDataAccess();
            IServersManager serversManager = new ServersManager(demoDataAccess);
            ServersController serversController = new ServersController(serversManager);

            // Act.
            ActionResult<string> actionResult = serversController.Delete(id);

            // Assert.
            ObjectResult expectedResult;
            if (demoDataAccess.DeleteServer(id))
            {
                expectedResult = new OkObjectResult(id);
            }
            else
            {
                expectedResult = new NotFoundObjectResult(id);
            }
            Assert.IsInstanceOfType(actionResult.Result, expectedResult.GetType());
        }
        [TestMethod]
        [DataRow("exists_serverID")]
        [DataRow("not_exists_serverID")]
        public void GetUrlTest(string id)
        {
            // Arrange.
            IDataAccess demoDataAccess = new DemoDataAccess();
            IServersManager serversManager = new ServersManager(demoDataAccess);
            ServersController serversController = new ServersController(serversManager);

            // Act.
            ActionResult<Server> actionResult = serversController.GetUrl(id);

            // Assert.
            ObjectResult expectedResult;
            Server server = demoDataAccess.GetServerByIdOfFlight(id);
            if (server != null)
            {
                expectedResult = new OkObjectResult(server);
            }
            else
            {
                expectedResult = new NotFoundObjectResult(id);
            }
            Assert.IsInstanceOfType(actionResult.Result, expectedResult.GetType());
        }
    }
}
