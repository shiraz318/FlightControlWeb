using FlightControlWeb;
using FlightControlWeb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static FlightControlWeb.Models.FlightPlan;

namespace UnitTestFlightControl
{
    [TestClass]
    class DemoDataAccess : IDataAccess
    {
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
            if (id.CompareTo("exists") == 0)
            {
                return CreateDemoFlightPlan();
            }
            return null;
        }
        // Generate fake flightPlan object.
        private FlightPlan CreateDemoFlightPlan()
        {
            FlightPlan flightPlan = new FlightPlan();

            flightPlan.CompanyName = "Demo_Company_Name";
            flightPlan.Passengers = 100;
            string time = "2025-05-05T05:05:05Z";
            DateTime initialTime = Convert.ToDateTime(time);
            flightPlan.InitialLocation = new Location(10, 10, initialTime);
            flightPlan.Segments = new List<Segment>();
            Segment segment = new Segment(10, 10, 10);
            flightPlan.Segments.Add(segment);

            return flightPlan;
        }

        public List<Flights> GetFlights(DateTime time)
        {
            List<Flights> flights = new List<Flights>();
            Flights flight = new Flights();
            flight.FlightId = "flight_id";
            flight.IsExternal = false;
            flight.Latitude = 0;
            flight.Longitude = 0;
            flight.Passengers = 10;
            flight.CompanyName = "company_name";
            flight.DateTime = time;
            flights.Add(flight);
            return flights;
        }

        public Server GetServerByIdOfFlight(string id)
        {
            throw new NotImplementedException();
        }

        public List<Server> GetServers()
        {
            List<Server> servers = new List<Server>();
            Server server = new Server();
            server.ServerId = "ServerID";
            server.ServerURL = "ServerUrl";
            servers.Add(server);
            return servers;

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
        public void DeleteServerFromExternalFlight(Server server)
        {
            throw new NotImplementedException();
        }
    }
}
