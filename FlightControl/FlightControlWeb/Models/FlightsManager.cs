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
            
            return s.DeleteFlight(id);
        }

        public Flights[] GetFlights(string dateTime, bool isExternal)
        {
            //Fake Number.
            Flights[] flights = new Flights[10];
            return flights;
        }
    }
}
