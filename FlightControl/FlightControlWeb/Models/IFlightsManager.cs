using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IFlightsManager
    {
        List<Flights> GetAllFlights(string dataTime, bool isExternal);
        bool DeleteFlight(string id);
    }
}
