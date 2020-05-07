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
 * Time- also in c# and in js needs to.
 * 
 * DateTime returns in json different format - no Z and +3
 * 
 * may be needs to uninstall the toolbox befor submit.
 * 
 * may be add an icon of X.
 * 
 * grira.
 * 
 * add external table - empty.
 * 
 * GetFlights implementaition - GetExternal(other servers), ConvertTime (time differences), linear interpulattion.
 * 
 * may be need to move all code of DataAccess to the dbContext or maybe just to have an instance of dataAccess.
 * 
 * Manager for Server.
 * 
 * map.
 *  
 * startup dependency injection.
 * 
 * do not forget you have a lot of dll files - maybe need to put them together in additional folder.
 */



