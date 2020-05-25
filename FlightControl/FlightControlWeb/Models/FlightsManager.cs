using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static FlightControlWeb.Models.FlightsManager;

namespace FlightControlWeb.Models
{
     public class FlightsManager : IFlightsManager
    {
        private IDataAccess dataAccess;

        public FlightsManager(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        public struct FlightsFromServers
        {
            public FlightsFromServers(List<Flights> flights, bool isError)
            {
                FlightsList = new List<Flights>();
                FlightsList = flights;
                IsError = isError;
            }
            public List<Flights> FlightsList { get; set; }
            public bool IsError { get; set; }
        }


        // Delete a Flight from the data base by a given id.
        public bool DeleteFlight(string id)
        {
            return dataAccess.DeleteFlightPlan(id);
        }

        private List<Flights> CreateFlightsFromServer(string strResult, DateTime time, Server server)
        {
            try
            {
                List<Flights> flights = new List<Flights>();
                var json = JArray.Parse(strResult);
                int i = 0;
                for (i = 0; i < json.Count; i++)
                {
                    Flights flight = new Flights();
                    flight.FlightId = json[i]["flight_id"].ToString();
                    flight.CompanyName = json[i]["company_name"].ToString();
                    flight.Latitude = Convert.ToDouble(json[i]["latitude"]);
                    flight.Longitude = Convert.ToDouble(json[i]["longitude"]);
                    flight.Passengers = Convert.ToInt32(json[i]["passengers"]);
                    flight.DateTime = time;
                    flight.IsExternal = true;
                    dataAccess.InsertExtenalFlightId(server, flight.FlightId);
                    flights.Add(flight);
                }
                return flights;
                // Server response is not a valid json file.
            }
            catch (Exception t)
            {
                string message = t.Message;
                return null;
            }
        }

        // Get a request from a server.
        private async Task<string> GetRequestFromServer(Server server, DateTime time)
        {
            string date = time.ToString("yyyy-MM-ddTHH:mm:ss") + "Z";
            string url = server.ServerURL;
            string command = url + "/api/Flights?relative_to=" + date;
            using var client = new HttpClient();
            TimeSpan timeout = new TimeSpan(0, 0, 0, 15);
            string strResult;
            client.Timeout = timeout;
            try
            {
                strResult = await client.GetStringAsync(command);
                return strResult;
            }
            // Server is not responsing in 15 seconds time.
            catch (Exception t)
            {
                string message = t.Message;
                return null;
            }

        }

        //// Get a flights from an external server relative to a given time.
        //private async Task<List<Flights>> GetRequestFromServer(Server server, DateTime time)
        //{
        //    string date = time.ToString("yyyy-MM-ddTHH:mm:ss") + "Z";
        //    string url = server.ServerURL;
        //    string command = url + "/api/Flights?relative_to=" + date;
        //    using var client = new HttpClient();
        //    TimeSpan timeout = new TimeSpan(0,0,0,15);
        //    string strResult;
        //    client.Timeout = timeout;
        //    try
        //    {
        //        strResult = await client.GetStringAsync(command);
        //    }
        //    // Server is not responsing in 15 seconds time.
        //    catch (Exception t)
        //    {
        //        string message = t.Message;
        //        return null;
        //    }
        //    try
        //    {
        //        List<Flights> flights = new List<Flights>();
        //        var json = JArray.Parse(strResult);
        //        int i = 0;
        //        for (i = 0; i < json.Count; i++)
        //        {
        //            Flights flight = new Flights();
        //            flight.FlightId = json[i]["flight_id"].ToString();
        //            flight.CompanyName = json[i]["company_name"].ToString();
        //            flight.Latitude = Convert.ToDouble(json[i]["latitude"]);
        //            flight.Longitude = Convert.ToDouble(json[i]["longitude"]);
        //            flight.Passengers = Convert.ToInt32(json[i]["passengers"]);
        //            flight.DateTime = time;
        //            flight.IsExternal = true;
        //            dataAccess.InsertExtenalFlightId(server, flight.FlightId);
        //            flights.Add(flight);
        //        }
        //        return flights;
        //        // Server response is not a valid json file.
        //    }catch(Exception t)
        //    {
        //        string message = t.Message;
        //        return null;
        //    }
        //}

        // Get flights from all the servers in the data base.
        public async Task<FlightsFromServers> GetFlightsFromServers(List<Server> servers,
            DateTime time)
        {
            List<Flights> flights = new List<Flights>();
            bool isError = false;
            //get flights.
            foreach (Server server in servers.ToList())
            {
                List<Flights> flights1 = new List<Flights>();
                string result = await GetRequestFromServer(server, time);
                // Server did not responded.
                if (result == null)
                {
                    continue;
                }

                flights1 = CreateFlightsFromServer(result, time, server);

                // Server response is invalid.
                if (flights1 == null)
                {
                    isError = true;
                }
                else
                {
                    flights.AddRange(flights1);
                }
            }
            FlightsFromServers flightsFromServers = new FlightsFromServers(flights, isError);
            return flightsFromServers;
        }

        // Get all the flights from the data base and from external servers if needed.
        public async Task<FlightsFromServers> GetAllFlights(string dateTime, bool isExternal)
        {
            DateTime time = DateTime.Parse(dateTime).ToUniversalTime();

            List<Flights> flights = new List<Flights>();
            List<Server> servers = new List<Server>();
 
            if (isExternal)
            {
                servers.AddRange(dataAccess.GetServers());

            }
            flights =  dataAccess.GetFlights(time);

            FlightsFromServers flightsFromServersInternal = new FlightsFromServers(flights, false);
            FlightsFromServers flightsFromServersExernal = 
                await GetFlightsFromServers(servers, time);

            flightsFromServersInternal.FlightsList.AddRange(flightsFromServersExernal.FlightsList);
            flightsFromServersInternal.IsError = flightsFromServersExernal.IsError;
           
            return flightsFromServersInternal;
        }
    }
}
