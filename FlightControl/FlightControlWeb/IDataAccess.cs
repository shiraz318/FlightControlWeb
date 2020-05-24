using FlightControlWeb.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb
{
    public interface IDataAccess
    {
        //void Create();
        List<Flights> GetFlights(DateTime time);
        Server GetServerByIdOfFlight(string id);
        List<Server> GetServers();
        FlightPlan GetFlightPlan(string id);
        bool DeleteServer(string id);
        bool DeleteFlightPlan(string id);
        void InsertExtenalFlightId(Server server, string id);
        void InsertFlightPlan(FlightPlan flightPlan, string id);
        void InsertServer(Server server);
    }
}
