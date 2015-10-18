using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using ProductCatalog.Models;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Logging;

namespace ProductCatalog
{
    public class Startup
    {

        public IConfiguration Configuration { get; private set; }
        public Startup(IApplicationEnvironment env)
        {
            var builder = new ConfigurationBuilder(env.ApplicationBasePath)
                        .AddJsonFile("config.json")
                        .AddEnvironmentVariables(); 
            Configuration = builder.Build();

        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            //setup options to use for controllers
            services.AddOptions(); 

            services.AddScoped<IConfigurationElasticClientSettings>(s =>
            {
                return new ConfigurationElasticClientSettings(Configuration.GetConfigurationSection("ElasticSearch"));
            });
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();

            //add basic logging
            //loggerFactory.MinimumLevel = LogLevel.Information;
            //loggerFactory.AddConsole();

            //Add Sample Data to Elastic Search
            var service = app.ApplicationServices.GetService<IConfigurationElasticClientSettings>();
            var helper = new ElasticSearchDataHelper(service);
            helper.AddTestData().Wait();

        }
    }
}
