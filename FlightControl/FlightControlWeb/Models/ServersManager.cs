using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class ServersManager :  IServersManager
    {
        private SQLiteDb s = new SQLiteDb(AppDomain.CurrentDomain.BaseDirectory + @"\Database.sqlite");

        // Post a server to the data base.
        public void Post(Server server)
        {
            s.InsertServer(server);
        }

        // Delete a server from the data base by a given id.
        public void Delete(string id)
        {
            s.DeleteServer(id);
        }

        // Get all the servers in the data base.
        public async Task<List<Server>> Get()
        {
            return await s.GetServers();
        }

        // Get a server by it's given Flght id.
        public async Task<Server> GetServerByIdOfFlight(string id)
        {
            return await s.GetServerByIdOfFlight(id);
        }
    }
}
