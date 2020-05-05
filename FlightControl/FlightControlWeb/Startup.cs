using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FlightControlWeb.Data;

namespace FlightControlWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRouting();
            services.AddControllers();

            services.AddDbContext<FlightControlWebContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("FlightControlWebContext")));

            //services.AddDbContext<FlightControlWebContext>(options =>
            //        options.UseSqlServer(Configuration.GetConnectionString("FlightControlWebContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            SQLiteDb.InitializeDatabase();
        }
    }
}

/*
 * TODO:
 * 
 * GetFlights implementaition.
 * 
 * present the flights on the web. - they sent some video in the telegram.
 * 
 * may be need to move all code of DataAccess to the dbContext or maybe just to have an instance of dataAccess.
 * 
 * Manager for Flights.
 * 
 * Manager for Server.
 * 
 * map.
 * 
 * DELETE flights/id may be meens to delete the flightplan.
 * 
 * startup dependency injection.
 * 
 * may by we do not need to create new flight when we get new flightplan and no need to save the flights in the database.
 * 
 * do not forget you have a lot of dll files - maybe need to put them together in additional folder.
 */