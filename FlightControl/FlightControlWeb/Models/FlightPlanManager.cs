using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanManager : IFlightPlanManager
    {
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
        public string setId()
        {
            Random rnd = new Random();
            long randomNamber = rnd.Next(100000, 1000000000);
            string id = CreateLetters() + randomNamber.ToString().Substring(0, 5) + CreateLetters();
            return id;
        }

        public Flights CreateFlight(FlightPlan fp)
        {
            Flights flight = new Flights();
            flight.FlightId = fp.Id;
            flight.Longitude = fp.InitialLocation.Longitude;
            flight.Latitude = fp.InitialLocation.Latitude;
            flight.Passengers = fp.Passengers;
            flight.CompanyName = fp.CompanyName;
            flight.DateTime = fp.InitialLocation.DateTime;
            flight.IsExternal = false;
            return flight;

        }
        public void AddFlightPlan(FlightPlan fp)
        {
            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
            fp.Id = setId();
            s.InsertFlightPlan(fp);
            Flights flights = CreateFlight(fp);
            s.InsertFlight(flights);
        }

        public FlightPlan GetFlightPlan(string id)
        {
            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
            return s.GetFlightPlan(id);
        }
    }
}
