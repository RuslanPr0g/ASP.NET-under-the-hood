using ASPNETunderthehood.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETunderthehood.Extentions
{
    public static class RoutingExtentions
    {
        public static IApplicationBuilder UseRoutingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoutingMiddleware>();
        }
    }
}
