using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace FlightControlWeb.Models
{
	[Serializable]
	public class Flights
	{
		private const int LongitudeBorder = 180;
		private const int LatitudeBorder = 90;

		private double longitude = LongitudeBorder + 1;
		private double latitude = LatitudeBorder + 1;
		private int passengers = -1;
		
		[Required]
		[JsonPropertyName("flight_id")]
		public string FlightId { get; set; }
		[Required]
		[JsonPropertyName("longitude")]
		public double Longitude { get { return longitude; } set { longitude = value; } }
		[Required]
		[JsonPropertyName("latitude")]
		public double Latitude { get { return latitude; } set { latitude = value; } }
		[Required]
		[JsonPropertyName("passengers")]
		public int Passengers { get { return passengers; } set { passengers = value; } }
		[Required]
		[JsonPropertyName("company_name")]
		public string CompanyName { get; set; }
		[Required]
		[JsonPropertyName("date_time")]
		public DateTime DateTime { get; set; }
		[Required]
		[JsonPropertyName("is_external")]
		public bool IsExternal { get; set; }

	}
}
