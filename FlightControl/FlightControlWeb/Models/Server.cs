using Newtonsoft.Json;
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
		public string ServerId { get ; set; }
		[Required]
		[JsonPropertyName("ServerURL")]
		public string ServerURL { get; set; }
		public override bool Equals(Object other)
		{
			Server otherServer = (Server)other;
			if (otherServer.ServerURL.CompareTo(this.ServerURL) == 0)
			{
				if (otherServer.ServerId.CompareTo(this.ServerId) == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
