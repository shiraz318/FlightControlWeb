using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models
{
	public class FlightPlan
	{
		[JsonPropertyName("passengers")]
		public int Passengers { get; set; }

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

			[JsonPropertyName("longitude")]
			public double Longitude { get; set; }

			[JsonPropertyName("latitude")]
			public double Latitude { get; set; }

			[JsonPropertyName("date_time")]
			public DateTime DateTime { get; set; }

			public override string ToString() => $"({Latitude}, {Longitude})";
		}

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

			[JsonPropertyName("longitude")]
			public double Longitude { get; set; }

			[JsonPropertyName("latitude")]
			public double Latitude { get; set; }

			[JsonPropertyName("timespan_seconds")]
			public int TimespanSeconds { get; set; }

			public override string ToString() => $"({Latitude}, {Longitude})";
		}

		private List<Segment> segments = new List<Segment>();
		[JsonPropertyName("segments")]
		public List<Segment> Segments
		{
			get { return segments; }
			set { segments = value; }
		}
		public string Id { get; set; }

		public FlightPlan(string id)
		{
			Id = id;
		}
		public FlightPlan()
		{
		}

	}


}

