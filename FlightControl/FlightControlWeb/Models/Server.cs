﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
	public class Server
	{
		[Required]
		[JsonPropertyName("ServerId")]
		public string ServerId { get; set; }
		[Required]
		[JsonPropertyName("ServerURL")]
		public string ServerURL { get; set; }
	}
}
