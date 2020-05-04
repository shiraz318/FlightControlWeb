using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class DummyFPManager : IFlightPlanManager
    {
        public void AddFlightPlan(FlightPlan fp)
        {
            DummyFP dfp = new DummyFP();
            dfp.CompanyName = fp.CompanyName;
            dfp.Passengers = fp.Passengers;
            dfp.Id = fp.Id;
            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
            s.InsertDummyFlightPlan(dfp);
        }

        public FlightPlan GetFlightPlan(string id)
        {
            SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
            FlightPlan fp = new FlightPlan(id);
            DummyFP dfp = s.GetDummyFP(id);
            fp.Passengers = dfp.Passengers;
            fp.CompanyName = dfp.CompanyName;
            fp.Id = dfp.Id;
            return fp;
        }
    }
}
