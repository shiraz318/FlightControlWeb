using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanManager : IFlightPlanManager
    {
        public void AddFlightPlan(FlightPlan fp)
        {
            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
            //s.InsertData(fp);
        }

        public FlightPlan GetFlightPlan(string id)
        {
            throw new NotImplementedException();
        }
    }
}
