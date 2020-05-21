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

	}


}

