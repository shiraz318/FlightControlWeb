﻿using System;
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
using FlightControlWeb.Models;

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

            services.AddScoped<IDataAccess, DataAccess>();
            services.AddScoped<IFlightsManager, FlightsManager>();
            services.AddScoped<IFlightPlanManager, FlightPlanManager>();
            services.AddScoped<IServersManager, ServersManager>();

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
            DataAccess.InitializeDatabase();
        }
    }
}

/*
 * TODO:
 *
 * may be needs to uninstall the toolbox befor submit.
 * 
 * may be need to move all code of DataAccess to the dbContext or maybe just to have an instance of dataAccess.
 *  
 * startup dependency injection.
 * 
 * do not forget you have a lot of dll files - maybe need to put them together in additional folder.
 *  
 *  delete all console.log before submitting.
 * End points events:
 *
 * invalid json file - no segmnents, and other things.
 * 
 * server/servers not active - we set timeout to check if resposed in time but what should we do in this case.
 * 
 * 
 * 
 * check death cyrcle.
 * unit tests.
 * 
 */



