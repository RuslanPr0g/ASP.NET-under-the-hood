using ASPNETunderthehood.Extentions;
using ASPNETunderthehood.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETunderthehood
{
    public class Startup
    {
        string token;
        readonly DefaultFilesOptions options = new DefaultFilesOptions();
        private IServiceCollection _services;

        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("us.poggers.com");
                options.ExcludedHosts.Add("www.pogchamp.com");
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 44326;
            });

            services.AddTransient<IMessageSender>(provider =>
            {

                if (DateTime.Now.Hour >= 12) return new EmailMessageSender();
                else return new SmsMessageSender();
            });

            services.AddTransient<ICounter>(provider =>
            {
                var counter = provider.GetService<RandomCounter>();
                return counter;
            });

            services.AddTransient<CounterService>();

            token = Configuration.GetValue<string>("Token");

            _services = services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMessageSender messageSender)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseToken(token);
                app.UseHsts();
            }

            var routeBuilder = new RouteBuilder(app);

            routeBuilder.MapRoute("{controller}/{action}",
                async context => {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync("bisegmental request");
                });


            routeBuilder.MapRoute("{controller}/{action}/{id}",
                async context => {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync("trisegmental request");
                });

            app.UseRouter(routeBuilder.Build());

            // ***

            app.UseErrorHandling();

            app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

            app.Map("/error", ap => ap.Run(async context =>
            {
                await context.Response.WriteAsync($"Err: {context.Request.Query["code"]}");
            }));

            //app.UseDirectoryBrowser();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            // create logger using logger factory ***

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddConsole();
                builder.AddFilter("System", LogLevel.Debug)
                    .AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Trace);
            });

            ILogger logger = loggerFactory.CreateLogger<Startup>();

            app.Run(async (context) =>
            {
                logger.LogInformation("Requested Path: {0}", context.Request.Path);
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
