using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{

	public class Flights
	{
		private string flightId;

		public string FlightId
		{
			get { return flightId; }
			set { flightId = value; }
		}
		private double longitude;

		public double Longitude
		{
			get { return longitude; }
			set { longitude = value; }
		}
		private double latitude;

		public double Latitude
		{
			get { return latitude; }
			set { latitude = value; }
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
		private string dateTime;

		public string DateTime
		{
			get { return dateTime; }
			set { dateTime = value; }
		}

		private bool isExternal;

		public bool IsExternal
		{
			get { return isExternal; }
			set { isExternal = value; }
		}
	}
}
