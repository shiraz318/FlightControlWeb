using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FlightControlWeb.Models;

namespace FlightControlWeb.Data
{
    public class FlightControlWebContext : DbContext
    {
        public FlightControlWebContext (DbContextOptions<FlightControlWebContext> options)
            : base(options)
        {
        }

        public DbSet<FlightControlWeb.Models.Server> Server { get; set; }
    }
}
