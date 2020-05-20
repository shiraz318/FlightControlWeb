using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FlightControlWeb.Models.FlightsManager;

namespace FlightControlWeb.Models
{
    public interface IFlightsManager
    {
        Task<FlightsFromServers> GetAllFlights(string dataTime, bool isExternal);
        Task<bool> DeleteFlight(string id);
    }
}
