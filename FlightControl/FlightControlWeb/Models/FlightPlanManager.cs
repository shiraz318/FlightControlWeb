using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static FlightControlWeb.Models.FlightPlan;

namespace FlightControlWeb.Models
{
    public class FlightPlanManager : IFlightPlanManager
    {
        private IDataAccess dataAccess;

        public FlightPlanManager(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        // Create the random letters in the id.
        private string CreateLetters()
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            int length = 2;

            char[] chars = new char[length];
            Random rd = new Random();
            int i;
            for (i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new String(chars);
        }
        
        // Set an unique id.
        private string SetId()
        {
            Random rnd = new Random();
            long randomNamber = rnd.Next(100000, 1000000000);
            string id = CreateLetters() + randomNamber.ToString().Substring(0, 5)
                + CreateLetters();
            return id;
        }

        // Send get request to a given url.
        private async Task<string> SendRequest(string id, string url)
        {
            try
            {
                string command = url + "/api/FlightPlan/" + id;
                using var client = new HttpClient();
                TimeSpan timeout = new TimeSpan(0, 0, 0, 50);
                client.Timeout = timeout;

                string content = await client.GetStringAsync(command);
                return content;
                // Server did not responsed in 15 seconds.
            }catch(Exception t)
            {
                string g = t.Message;
                return null;
            }

        }

        // Create a FlightPlan object from a json file.
        private FlightPlan CreateFlightPlanFromJson(string strResult)
        {
            FlightPlan flightPlan = new FlightPlan();
            var json = JObject.Parse(strResult);
            flightPlan.CompanyName = json["company_name"].ToString();
            double longitude = Convert.ToDouble(json["initial_location"]["longitude"]);
            double latitude = Convert.ToDouble(json["initial_location"]["latitude"]);
            DateTime dateTime = Convert.ToDateTime(json["initial_location"]["date_time"]);
            Location location = new Location(longitude, latitude, dateTime);
            flightPlan.InitialLocation = location;
            flightPlan.Passengers = Convert.ToInt32(json["passengers"]);
            List<Segment> segments = new List<Segment>();
            int i = 0;
            JArray items = (JArray)json["segments"];
            int length = items.Count;
            // Create segments.
            for (i = 0; i < length; i++)
            {
                double longitude1 = Convert.ToDouble(json["segments"][i]["longitude"]);
                double latitude1 = Convert.ToDouble(json["segments"][i]["latitude"]);
                int timespnaSeconds = 
                    Convert.ToInt32(json["segments"][i]["timespan_seconds"]);
                Segment segment = new Segment(longitude1, latitude1, timespnaSeconds);
                segments.Add(segment);
            }
            flightPlan.Segments = segments;
            return flightPlan;
        }

        // Get a FlightPlan from a server with a given url and id.
        public async Task<FlightPlan> GetFlightPlanFromServer(string id, string url)
        {
            string strResult = await SendRequest(id, url);
            if (strResult == null)
            {
                return null;
            }
            try
            {
                return CreateFlightPlanFromJson(strResult);
            } catch(Exception e)
            {
                string message = e.Message;
                return null;
            }
        }

        // Add a given FlightPlan into the data base.
        public string AddFlightPlan(FlightPlan fp)
        {
            string id = SetId();

            dataAccess.InsertFlightPlan(fp, id);
            return id;
        }

        // Get a FlightPlan by a given id.
        public async Task<FlightPlan> GetFlightPlan(string id)
        {      
            FlightPlan flightPlan = dataAccess.GetFlightPlan(id); ;
            if (flightPlan != null)
            {
                return flightPlan;
            }
            Server server = dataAccess.GetServerByIdOfFlight(id);
            if (server != null)
            {
                return await GetFlightPlanFromServer(id, server.ServerURL);
            }
            return null;
        }

    }
}
