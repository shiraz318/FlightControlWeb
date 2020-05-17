using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IServersManager
    {
        void Post(Server server);
        void Delete(string id);
        Task<List<Server>> Get();
        Task<Server> GetServerByIdOfFlight(string id);
    }
}
