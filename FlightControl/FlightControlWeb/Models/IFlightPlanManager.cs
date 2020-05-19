using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IFlightPlanManager
    {
        Task<string> AddFlightPlan(FlightPlan fp);
        Task<FlightPlan> GetFlightPlan(string id);
        //Task<FlightPlan[]> GetAllFlightPlans();
        FlightPlan GetFlightPlanFromServer(string id, string url);
    }
}
