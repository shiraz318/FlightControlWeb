using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
	public class FlightPlan
	{
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

		public struct Location
		{
			public Location(double longitudeStruct, double latitudeStruct, string dateTimeStruct)
			{
				Longitude = longitudeStruct;
				Latitude = latitudeStruct;
				DateTime = dateTimeStruct;
			}

			public double Longitude { get; set; }
			public double Latitude { get; set; }
			public string DateTime { get; set; }

			public override string ToString() => $"({Latitude}, {Longitude})";
		}
		private Location initialLocation;

		public Location InitialLocation
		{
			get { return initialLocation; }
			set { initialLocation = value; }
		}

		public struct Segment
		{
			public Segment(double longitudeStruct, double latitudeStruct, int timespanSecondsStruct)
			{
				Longitude = longitudeStruct;
				Latitude = latitudeStruct;
				TimespanSeconds = timespanSecondsStruct;
			}

			public double Longitude { get; set; }
			public double Latitude { get; set; }
			public int TimespanSeconds { get; set; }

			public override string ToString() => $"({Latitude}, {Longitude})";
		}

		//private ArraySegment<Segment> segments = new ArraySegment<FlightPlan.Segment>();

		//public ArraySegment<Segment> Segments
		//{
		//	get { return segments; }
		//	set { segments = value; }
		//}


		//ArraySegment<Segment> segments = new ArraySegment<Segment>();

		private string id;

		public string Id
		{
			get { return id; }
			set { id = value; }
		}

		public FlightPlan(string id)
		{
			Id = id;
		}
		public FlightPlan()
		{
		}

	}


}

