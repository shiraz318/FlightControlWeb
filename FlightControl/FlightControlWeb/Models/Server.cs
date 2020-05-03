using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
	public class Server
	{
		private string serverId;

		public string ServerId
		{
			get { return serverId; }
			set { serverId = value; }
		}
		private string serverURL;

		public string ServerURL
		{
			get { return serverURL; }
			set { serverURL = value; }
		}

	}
}
