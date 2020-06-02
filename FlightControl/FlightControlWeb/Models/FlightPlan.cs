using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FlightControlWeb.Models
{
	public class FlightPlan
	{
		private const int LongitudeBorder = 180;
		private const int LatitudeBorder = 90;

		private int passengers = -1;
		private Location location = new Location(LongitudeBorder + 1, LatitudeBorder + 1, new DateTime());
		private List<Segment> segments = new List<Segment>();


		[Required]
		[JsonPropertyName("passengers")]
		public int Passengers { get { return passengers; } set { passengers = value; } }
		[Required]
		[JsonPropertyName("company_name")]
		public string CompanyName { get; set; }

		public struct Location
		{
			private double longitude;
			private double latitude;
			public Location(double longitudeStruct, double latitudeStruct, DateTime dateTimeStruct) :this()
			{
				this.longitude = LongitudeBorder + 1;
				this.latitude = LatitudeBorder + 1;
				Longitude = longitudeStruct;
				Latitude = latitudeStruct;
				DateTime = dateTimeStruct;
			}
			[Required]
			[JsonPropertyName("longitude")]
			public double Longitude { get { return longitude; } set { longitude = value; } }
			[Required]
			[JsonPropertyName("latitude")]
			public double Latitude { get { return latitude; } set { latitude = value; } }
			//[Required]
			//[JsonPropertyName("date_time")]
			public DateTime DateTime { get; set; }
			[Required]
			[JsonPropertyName("date_time")]
			public string StringDateTime { get { return this.DateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"); } set {this.DateTime = DateTime.Parse(value).ToUniversalTime(); } }

			public override string ToString() => $"({Latitude}, {Longitude})";
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
			public override bool Equals(Object other)
			{
				Location otherLocation = (Location)other;
				if (this.Latitude != otherLocation.Latitude)
				{
					return false;
				}
				if (this.Longitude != otherLocation.Longitude)
				{
					return false;
				}
				if (this.DateTime.CompareTo(otherLocation.DateTime) != 0)
				{
					return false;
				}
				
				return true;
			}
		}

		[Required]
		[JsonPropertyName("initial_location")]
		public Location InitialLocation { get { return location; } set { location = value; } }

		public struct Segment
		{
			private double longitude;
			private double latitude;
			private int timespanSeconds;
			public Segment(double longitudeStruct, double latitudeStruct, int timespanSecondsStruct) : this()
			{
				this.longitude = LongitudeBorder + 1;
				this.latitude = LatitudeBorder + 1;
				this.timespanSeconds = -1;
				Longitude = longitudeStruct;
				Latitude = latitudeStruct;
				TimespanSeconds = timespanSecondsStruct;
			}
			[Required]
			[JsonPropertyName("longitude")]
			public double Longitude { get { return longitude; } set { longitude = value; } }
			[Required]
			[JsonPropertyName("latitude")]
			public double Latitude { get { return latitude; } set { latitude = value; } }
			[Required]
			[JsonPropertyName("timespan_seconds")]
			public int TimespanSeconds { get { return timespanSeconds; } set { timespanSeconds = value; } }

			public override string ToString() => $"({Latitude}, {Longitude})";
			public override bool Equals(Object other)
			{
				Segment otherSegment = (Segment)other;
				if (this.Latitude != otherSegment.Latitude)
				{
					return false;
				}
				if (this.Longitude != otherSegment.Longitude)
				{
					return false;
				}
				if (this.TimespanSeconds != otherSegment.TimespanSeconds)
				{
					return false;
				}
				return true;
			}
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

		}

		[Required]
		[JsonPropertyName("segments")]
		public List<Segment> Segments
		{
			get { return segments; }
			set { segments = value; }
		}

		public FlightPlan()
		{
		}
		public FlightPlan(int passengers, string companyName, Location location, List<Segment> segments)
		{
			this.passengers = passengers;
			this.CompanyName = companyName;
			this.InitialLocation = location;
			this.Segments = segments;
		}
		public override bool Equals(Object other)
		{
			FlightPlan otherFlightPlan = (FlightPlan)other;
			if (this.CompanyName.CompareTo(otherFlightPlan.CompanyName) != 0)
			{
				return false;
			}
			if (this.Passengers != otherFlightPlan.Passengers)
			{
				return false;
			}
			if (!(this.InitialLocation.Equals(otherFlightPlan.InitialLocation)))
			{
				return false;
			}
			int i = 0;
			int size = this.Segments.Count <= otherFlightPlan.Segments.Count ?
				this.Segments.Count : otherFlightPlan.Segments.Count;
			for (i = 0; i < size; i++)
			{
				Segment thisSegment = this.Segments[i];
				Segment otherSegment = otherFlightPlan.Segments[i];
				
				if (! thisSegment.Equals(otherSegment))
				{
					return false;
				}
			}

			return true;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

	}


}

