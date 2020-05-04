using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class DummyFP
    {

		private string id;

		public string Id
		{
			get { return id; }
			set { id = value; }
		}
		private int passengers;

		public int Passengers
		{
			get { return passengers; }
			set { passengers = value; }
		}
		private string companyName;

		public string CompanyName
		{
			get { return companyName; }
			set { companyName = value; }
		}
	}
}
