using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class ServersManager :  IServersManager
    {
        private IDataAccess s;

        public ServersManager(IDataAccess dataAccess)
        {
            s = dataAccess;
        }

        // Post a server to the data base.
        public void Post(Server server)
        {
            s.InsertServer(server);
        }

        // Delete a server from the data base by a given id.
        public bool Delete(string id)
        {
            return s.DeleteServer(id);
        }

        // Get all the servers in the data base.
        public List<Server> Get()
        {
            return s.GetServers();
        }

        // Get a server by it's given Flght id.
        public Server GetServerByIdOfFlight(string id)
        {
            return  s.GetServerByIdOfFlight(id);
        }
    }
}
