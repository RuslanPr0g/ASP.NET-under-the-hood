using ASPNETunderthehood.Extentions;
using ASPNETunderthehood.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                options.ExcludedHosts.Add("us.example.com");
                options.ExcludedHosts.Add("www.example.com");
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 44326;
            });

            services.AddTransient<IMessageSender, EmailMessageSender>();

            services.AddTransient<ICounter>(provider => {
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

            app.UseErrorHandling();

            app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

            app.Map("/error", ap => ap.Run(async context =>
            {
                await context.Response.WriteAsync($"Err: {context.Request.Query["code"]}");
            }));

            app.UseAuthentication();

            //app.UseDefaultFiles(UseDefaultFile("hello.html"));

            //app.UseDirectoryBrowser();

            //app.UseStaticFiles();

            //app.UseHttpsRedirection();

            app.UseCounter();

            //app.Run(async context =>
            //{
            //    var sb = new StringBuilder();
            //    sb.Append("<h1>Servises</h1>");
            //    sb.Append("<table>");
            //    sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Realization</th></tr>");
            //    foreach (var svc in _services)
            //    {
            //        sb.Append("<tr>");
            //        sb.Append($"<td>{svc.ServiceType.FullName}</td>");
            //        sb.Append($"<td>{svc.Lifetime}</td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            //        sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
            //        sb.Append("</tr>");
            //    }
            //    sb.Append("</table>");
            //    context.Response.ContentType = "text/html;charset=utf-8";
            //    await context.Response.WriteAsync(sb.ToString());
            //});
        }

        private DefaultFilesOptions UseDefaultFile(string file)
        {
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add(file);
            return options;
        }
    }
}
