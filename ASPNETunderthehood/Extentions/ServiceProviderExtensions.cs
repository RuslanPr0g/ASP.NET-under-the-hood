using ASPNETunderthehood.Services.TimeServices;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNETunderthehood.Extentions
{
    public static class ServiceProviderExtensions
    {
        public static void AddTimeService(this IServiceCollection services)
        {
            services.AddTransient<TimeService>();
        }
    }
}
