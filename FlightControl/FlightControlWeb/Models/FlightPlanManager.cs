using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanManager : IFlightPlanManager
    {
        //private SQLiteDb s = new SQLiteDb(Environment.CurrentDirectory + @"\Database.sqlite");
        private SQLiteDb s = new SQLiteDb(AppDomain.CurrentDomain.BaseDirectory + @"\Database.sqlite");
        private string CreateLetters()
        {
           
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            int length = 2;

            char[] chars = new char[length];
            Random rd = new Random();
            int i;
            for (i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new String(chars);
        }
        public string setId()
        {
            Random rnd = new Random();
            long randomNamber = rnd.Next(100000, 1000000000);
            string id = CreateLetters() + randomNamber.ToString().Substring(0, 5) + CreateLetters();
            return id;
        }

       
        public string AddFlightPlan(FlightPlan fp)
        {
            string id = setId();

            s.InsertFlightPlan(fp, id);
            return id;
            //Flights flights = CreateFlight(fp);
            //s.InsertFlight(flights);
        }

        public FlightPlan GetFlightPlan(string id)
        {      
            return  s.GetFlightPlan(id);
        }

        public FlightPlan[] GetAllFlightPlans()
        {
            List<FlightPlan> flightPlans1 = s.GetAllFlightPlans();
            FlightPlan[] flightPlans = new FlightPlan[flightPlans1.Count];
            int i = 0;
            foreach (FlightPlan fp in flightPlans1)
            {
                flightPlans[i] = fp;
                i++;
            }

            return flightPlans;
        }
    }
}
