using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IFlightsManager
    {
        Flights[] GetFlights(string dateTime, bool isExternal);
        bool DeleteFlight(string id);
    }
}
