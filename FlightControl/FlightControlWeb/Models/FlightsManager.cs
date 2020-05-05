using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightsManager : IFlightsManager
    {
        public bool DeleteFlight(string id)
        {
            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
            
            return s.DeleteFlightPlan(id);
        }


        public List<Flights> GetAllFlights(string dateTime, bool isExternal)
        {
            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
            string time = dateTime.Substring(1, dateTime.Length - 1);
            List<Flights> flights = new List<Flights>();
            flights = s.GetFlights(time, isExternal);
            return flights;
        }
    }
}
