using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;
using NLog.Web;
using STARC.Sample.Service.Configs;
using STARC.Sample.Service.DataServices;
using STARC.Sample.Service.DBContexts;
using STARC.Sample.Service.Repositories;
using STARC.Sample.Service;
using Microsoft.Extensions.Options;

namespace STARC.Sample.Service
{
    public class Startup
    {
        bool enableSwagger;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            bool.TryParse(Configuration["EnableSwagger"], out enableSwagger);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //OAuth Configuration
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //Auth0 configuration settings
                options.Authority = $"{Uri.UriSchemeHttps}://{Configuration["Auth0:Domain"]}";
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.Configure<ConfigSettings>(Configuration);

            //DBContext Registration
            services.AddDbContext<PersonsContext>();

            //DI custom interface registration
            services.AddTransient<IPersonsService, PersonsService>();
            services.AddTransient<IPersonsRepository, PersonsRepository>();
            

            if (enableSwagger)
            {
                services.AddSwaggerGen(options =>
                {
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                    options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "STARC Sample Service" });
                    options.DescribeAllEnumsAsStrings();
                    options.IncludeXmlComments(basePath + "STARC.Sample.Service.xml");
                });
            }

            /********* For AutoFac Configuration: Add Autofac.Extensions.DependencyInjection Extension  ************/
            //var container = new ContainerBuilder();
            //container.Populate(services);

            //return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();

            /***** Swagger Configuration ********/
            if (enableSwagger)
            {
                app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "STARC Sample Service");
                    options.DocExpansion("full");

                });

            }

            /********** Logger Configuration. *******/
            //Console           loggerFactory.AddConsole();
            //Debug             loggerFactory.AddDebug();
            //EventSource       loggerFactory.AddEventSourceLogger();
            //EventLog          logging.AddEventLog();      // Requires Microsoft.Extensions.Logging.EventLog
            //TraceSource       logging.AddTraceSource();   //Requires  Microsoft.Extensions.Logging.TraceSource
            //Azure App Service                             //Microsoft.Extensions.Logging.AzureAppServices 

            //NLog Configuration.
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog("Configs/nLog.config");
        }
    }
}
