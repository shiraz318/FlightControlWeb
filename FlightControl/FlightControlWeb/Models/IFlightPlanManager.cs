using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IFlightPlanManager
    {
        string AddFlightPlan(FlightPlan fp);
        Task<FlightPlan> GetFlightPlan(string id);
        Task<FlightPlan> GetFlightPlanFromServer(string id, string url);
    }
}
