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
		[Required]
		[JsonPropertyName("flight_id")]
		public string FlightId { get; set; }
		[Required]
		[JsonPropertyName("longitude")]
		public double Longitude { get; set; }
		[Required]
		[JsonPropertyName("latitude")]
		public double Latitude { get; set; }
		[Required]
		[JsonPropertyName("passengers")]
		public int Passengers { get; set; }
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
