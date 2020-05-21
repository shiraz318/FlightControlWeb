using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static FlightControlWeb.Models.FlightsManager;

namespace FlightControlWeb.Models
{
   

     public class FlightsManager : IFlightsManager
    {

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

        private SQLiteDb s = new SQLiteDb(AppDomain.CurrentDomain.BaseDirectory + @"\Database.sqlite");

        // Delete a Flight from the data base by a given id.
        public bool DeleteFlight(string id)
        {
            return s.DeleteFlightPlan(id);
        }

        // Get a flights from an external server relative to a given time.
        private async Task<List<Flights>> GetRequestFromServer(Server server, DateTime time)
        {
            string date = time.ToString("yyyy-MM-ddTHH:mm:ss") + "Z";
            string url = server.ServerURL;
            string command = url + "/api/Flights?relative_to=" + date;
            using var client = new HttpClient();

            TimeSpan timeout = new TimeSpan(0,0,0,20);
            string strResult;
            client.Timeout = timeout;
            try
            {
                 strResult = await client.GetStringAsync(command);
            } catch(Exception t)
            {
                return null;
            }
            
            //Uri myUri = new Uri(command, UriKind.Absolute);

            //WebRequest request = WebRequest.Create(command);
            //request.Method = "GET";
            //HttpWebResponse response = null;
            //response = (HttpWebResponse)request.GetResponse();

            //string strResult = null;

            //using (Stream stream = response.GetResponseStream())
            //{
            //    StreamReader streamReader = new StreamReader(stream);
            //    strResult = streamReader.ReadToEnd();
            //    streamReader.Close();
            //}
            
            List<Flights> flights = new List<Flights>();
            try
            {
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
                    s.InsertExtenalFlightId(server, flight.FlightId);
                    flights.Add(flight);
                }
            } catch(Exception e)
            {
                var json = JObject.Parse(strResult);
                return null;
            
        }
            return flights;
        }

        // Get flights from all the servers in the data base.
        public async Task<FlightsFromServers> GetFlightsFromServers(List<Server> servers, DateTime time)
        {
            List<Flights> flights = new List<Flights>();
            bool isError = false;
            //get flights.
            foreach (Server server in servers.ToList())
            {
                List<Flights> flights1 = new List<Flights>();
                flights1 = await GetRequestFromServer(server, time);
                // Server did not responsed.
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

            //string str = time.ToString();
            List<Flights> flights = new List<Flights>();
            List<Server> servers = new List<Server>();
            //List<FlightPlan> flightPlans = new List<FlightPlan>();
            // flightPlans = s.GetAllFlightPlans(isExternal);
            if (isExternal)
            {
                servers = s.GetServers();
            }
            flights =  s.GetFlights(time);
            FlightsFromServers flightsFromServersInternal = new FlightsFromServers(flights, false);
            FlightsFromServers flightsFromServersExernal = await GetFlightsFromServers(servers, time);

            flightsFromServersInternal.FlightsList.AddRange(flightsFromServersExernal.FlightsList);
            flightsFromServersInternal.IsError = flightsFromServersExernal.IsError;

            return flightsFromServersInternal;
        }
    }
}
