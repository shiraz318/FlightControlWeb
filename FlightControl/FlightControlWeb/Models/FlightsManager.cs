using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlightControlWeb.Models
{
    
     public class FlightsManager : IFlightsManager
    {
        //private SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
        private SQLiteDb s = new SQLiteDb(AppDomain.CurrentDomain.BaseDirectory + @"\Database.sqlite");
        public bool DeleteFlight(string id)
        {
            return s.DeleteFlightPlan(id);
        }


        public List<Flights> DeserializeFlightsFromJson(string json)
        {
            List<Flights> flightsToReturn = new List<Flights>();

            string[] flights = json.Split('{');
            foreach(string flight in flights)
            {
                Flights f = new Flights();

            }

            return flightsToReturn;
        }



        public List<Flights> GetRequestFromServer(Server server, DateTime time)
        {
            string date = time.ToString("yyyy-MM-ddTHH:mm:ss") + "Z";
            string url = server.ServerURL;
            string command = url + "/api/Flights?relative_to=" + date;
            //List<Flights> flights = new List<Flights>();

            WebRequest request = WebRequest.Create(command);
            request.Method = "GET";
            HttpWebResponse response = null;
            response = (HttpWebResponse)request.GetResponse();

            string strResult = null;

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader streamReader = new StreamReader(stream);
                strResult = streamReader.ReadToEnd();
                streamReader.Close();
            }
            
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
                flights.Add(flight);
            }
            ////http://localhost:61896/api/Flights?relative_to=2020-05-13T10:39:00Z&sync_all
          
            return flights;
        }

        public List<Flights> GetFlightsFromServers(List<Server> servers, DateTime time)
        {
            List<Flights> flights = new List<Flights>();
            //get flights.
            foreach (Server server in servers)
            {
                flights.AddRange(GetRequestFromServer(server,time));
            }
            return flights;
        }


        public List<Flights> GetAllFlights(string dateTime, bool isExternal)
        {

            DateTime time = DateTime.Parse(dateTime).ToUniversalTime();

            string str = time.ToString();
            List<Flights> flights = new List<Flights>();
            List<Server> servers = new List<Server>();
            //List<FlightPlan> flightPlans = new List<FlightPlan>();
            // flightPlans = s.GetAllFlightPlans(isExternal);
            if (isExternal)
            {
                servers = s.GetServers();
            }
            flights = s.GetFlights(time);
            flights.AddRange(GetFlightsFromServers(servers, time));
            return flights;
        }
    }
}
