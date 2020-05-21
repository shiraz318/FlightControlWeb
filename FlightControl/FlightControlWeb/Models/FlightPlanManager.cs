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
        private SQLiteDb s = new SQLiteDb(AppDomain.CurrentDomain.BaseDirectory + @"\Database.sqlite");

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
            string id = CreateLetters() + randomNamber.ToString().Substring(0, 5) + CreateLetters();
            return id;
        }

        // Send get request to a given url.
        private async Task<string> SendRequest(string id, string url)
        {
            string command = url + "/api/FlightPlan/" + id;
            using var client = new HttpClient();

            string content = await client.GetStringAsync(command);
            return content;

           
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
            //return strResult;
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
            for (i = 0; i < length; i++)
            {
                double longitude1 = Convert.ToDouble(json["segments"][i]["longitude"]);
                double latitude1 = Convert.ToDouble(json["segments"][i]["latitude"]);
                int timespnaSeconds = Convert.ToInt32(json["segments"][i]["timespan_seconds"]);
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

            return CreateFlightPlanFromJson(strResult);
        }

        // Add a given FlightPlan into the data base.
        public string AddFlightPlan(FlightPlan fp)
        {
            string id = SetId();

            s.InsertFlightPlan(fp, id);
            return id;
        }

        // Get a FlightPlan by a given id.
        public FlightPlan GetFlightPlan(string id)
        {      
            return s.GetFlightPlan(id);
        }

    }
}
