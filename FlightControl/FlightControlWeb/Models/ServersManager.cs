using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class ServersManager :  IServersManager
    {
        private SQLiteDb s = new SQLiteDb(AppDomain.CurrentDomain.BaseDirectory + @"\Database.sqlite");
        public void Post(Server server)
        {
            s.InsertServer(server);
        }
        public void Delete(string id)
        {
            s.DeleteServer(id);
        }
        public async Task<List<Server>> Get()
        {
            return s.GetServers();
        }
        public async Task<Server> GetServerByIdOfFlight(string id)
        {
            return s.GetServerByIdOfFlight(id);
        }
    }
}
