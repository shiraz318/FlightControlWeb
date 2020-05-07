using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightsManager : IFlightsManager
    {
        private SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
        //private SQLiteDb s = new SQLiteDb(AppDomain.CurrentDomain.BaseDirectory + @"\Database.sqlite");
        public bool DeleteFlight(string id)
        {
            return s.DeleteFlightPlan(id);
        }


        public List<Flights> GetAllFlights(string dateTime, bool isExternal)
        { 
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
