using FlightControlWeb;
using FlightControlWeb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static FlightControlWeb.Models.FlightPlan;

namespace FlightControlWebTest
{
    [TestClass]
    class FakeDataAccess : IDataAccess
    {
        public void Create()
        {
            throw new NotImplementedException();
        }

        public bool DeleteFlightPlan(string id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteServer(string id)
        {
            throw new NotImplementedException();
        }

        public FlightPlan GetFlightPlan(string id)
        {

            FlightPlan flightPlan = new FlightPlan();

            flightPlan.CompanyName = "";
            flightPlan.Passengers = 0;
            flightPlan.InitialLocation = new Location(0,0,new DateTime());
            flightPlan.Segments = new List<Segment>();
            

            return flightPlan;
        }

        public List<Flights> GetFlights(DateTime time)
        {
            throw new NotImplementedException();
        }

        public Server GetServerByIdOfFlight(string id)
        {
            throw new NotImplementedException();
        }

        public List<Server> GetServers()
        {
            throw new NotImplementedException();
        }

        public void InsertExtenalFlightId(Server server, string id)
        {
            throw new NotImplementedException();
        }

        public void InsertFlightPlan(FlightPlan flightPlan, string id)
        {
            throw new NotImplementedException();
        }

        public void InsertServer(Server server)
        {
            throw new NotImplementedException();
        }
    }
}
