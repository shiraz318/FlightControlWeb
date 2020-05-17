using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IFlightsManager
    {
        Task<List<Flights>> GetAllFlights(string dataTime, bool isExternal);
        Task<bool> DeleteFlight(string id);
    }
}
