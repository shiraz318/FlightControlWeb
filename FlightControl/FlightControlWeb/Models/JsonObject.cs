using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class JsonObject
    {
		private List<Flights> flights = new List<Flights>();
	
		public List<Flights> Flights
		{
			get { return flights; }
			set { flights = value; }
		}
	}
}

