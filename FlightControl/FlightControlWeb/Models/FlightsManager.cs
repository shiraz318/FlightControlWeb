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



        public List<Flights> GetRequestFromServer(Server server)
        {
            string date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + "Z";
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
            Console.WriteLine(strResult);
                
            List<Flights> flights = JsonConvert.DeserializeObject<List<Flights>>(strResult);

            //var dtoprojects = new List<Flights>();

            //using (var client = new HttpClient())
            //{
            //    var uri = command;

            //    var response = client.GetAsync(uri).Result;

            //    if (!response.IsSuccessStatusCode)
            //        throw new Exception(response.ToString());

            //    var responseContent = response.Content;
            //    var responseString = responseContent.ReadAsStringAsync().Result;

            //    dynamic projects = JArray.Parse(responseString) as JArray;

            //    foreach (var obj in projects)
            //    {
            //        Flights dto = obj.ToObject<Flights>();

            //        dtoprojects.Add(dto);
            //    }

            //    return dtoprojects;
            //}







            //    WebRequest request = WebRequest.Create(command);
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

            ////http://localhost:61896/api/Flights?relative_to=2020-05-13T09:55:00Z&sync_all
            //List<Flights> flights = new List<Flights>();
            ////JsonObject jo = JsonConvert.DeserializeObject<JsonObject>(strResult);
            ////var listOfObjectsResult = Json.Decode<List<Flights>>(strResult);
            ////    List<Flights> flights = JsonConvert.DeserializeObject<List<Flights>>(strResult);
            ////List<Flights> flights = DeserializeFlightsFromJson(strResult);

            //var responseContent = response.Content;
            //var responseString = responseContent.ReadAsStringAsync().Result;
            //dynamic projects = JArray.Parse(strResult) as JArray;

            //foreach (var obj in projects)
            //{
            //    Flights dto = obj.ToObject<Flights>();

            //    flights.Add(dto);
            //}
            //// Set the isExternal property to true.
            //foreach (Flights f in flights)
            //{
            //    f.IsExternal = true;
            //}
            return flights;
        }

        public List<Flights> GetFlightsFromServers(List<Server> servers )
        {
            List<Flights> flights = new List<Flights>();
            //get flights.
            foreach (Server server in servers)
            {
                flights.AddRange(GetRequestFromServer(server));
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
            flights.AddRange(GetFlightsFromServers(servers));
            return flights;
        }
    }
}
