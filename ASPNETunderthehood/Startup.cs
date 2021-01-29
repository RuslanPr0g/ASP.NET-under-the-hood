using ASPNETunderthehood.Extentions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETunderthehood
{
    public class Startup
    {
        string token;

        DefaultFilesOptions options = new DefaultFilesOptions();

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

            token = Configuration.GetValue<string>("Token");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseToken(token);
            }

            app.UseErrorHandling();

            app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

            app.Map("/error", ap => ap.Run(async context =>
            {
                await context.Response.WriteAsync($"Err: {context.Request.Query["code"]}");
            }));

            app.UseAuthentication();

            app.UseDefaultFiles(UseDefaultFile("hello.html"));

            app.UseDirectoryBrowser();

            app.UseStaticFiles();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World");
            });
        }

        private DefaultFilesOptions UseDefaultFile(string file)
        {
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add(file);
            return options;
        }
    }
}
