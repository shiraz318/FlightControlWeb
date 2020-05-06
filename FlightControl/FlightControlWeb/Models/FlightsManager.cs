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
            // Remove the <>.
            DateTime time = DateTime.Parse(dateTime.Substring(1, dateTime.Length - 2));
            List<Flights> flights = new List<Flights>();
            //List<FlightPlan> flightPlans = new List<FlightPlan>();
           // flightPlans = s.GetAllFlightPlans(isExternal);
            flights = s.GetFlights(time, isExternal);
            return flights;
        }
    }
}
