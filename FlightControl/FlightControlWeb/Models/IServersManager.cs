using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IServersManager
    {
        void Post(Server server);
        bool Delete(string id);
        List<Server> Get();
        Server GetServerByIdOfFlight(string id);
    }
}
