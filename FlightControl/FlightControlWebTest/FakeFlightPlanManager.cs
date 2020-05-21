using FlightControlWeb;
using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlightControlWebTest
{
    class FakeFlightPlanManager : IFlightPlanManager
    {
        private IDataAccess dataAccess;

        public FakeFlightPlanManager(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public string AddFlightPlan(FlightPlan fp)
        {
            throw new NotImplementedException();
        }

        public FlightPlan GetFlightPlan(string id)
        {
            return dataAccess.GetFlightPlan(id);
        }

        public Task<FlightPlan> GetFlightPlanFromServer(string id, string url)
        {
            throw new NotImplementedException();
        }
    }
}
