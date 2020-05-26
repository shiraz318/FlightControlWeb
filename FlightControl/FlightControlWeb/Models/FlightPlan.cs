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
		[Required]
		[JsonPropertyName("passengers")]
		public int Passengers { get; set; }
		[Required]
		[JsonPropertyName("company_name")]
		public string CompanyName { get; set; }

		public struct Location
		{
			public Location(double longitudeStruct, double latitudeStruct, DateTime dateTimeStruct)
			{
				Longitude = longitudeStruct;
				Latitude = latitudeStruct;
				DateTime = dateTimeStruct;
			}
			[Required]
			[JsonPropertyName("longitude")]
			public double Longitude { get; set; }
			[Required]
			[JsonPropertyName("latitude")]
			public double Latitude { get; set; }
			[Required]
			[JsonPropertyName("date_time")]
			public DateTime DateTime { get; set; }

			public override string ToString() => $"({Latitude}, {Longitude})";
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
		public Location InitialLocation { get; set; }

		public struct Segment
		{
			public Segment(double longitudeStruct, double latitudeStruct, int timespanSecondsStruct)
			{
				Longitude = longitudeStruct;
				Latitude = latitudeStruct;
				TimespanSeconds = timespanSecondsStruct;
			}
			[Required]
			[JsonPropertyName("longitude")]
			public double Longitude { get; set; }
			[Required]
			[JsonPropertyName("latitude")]
			public double Latitude { get; set; }
			[Required]
			[JsonPropertyName("timespan_seconds")]
			public int TimespanSeconds { get; set; }

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

		}

		private List<Segment> segments = new List<Segment>();
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


	}


}

