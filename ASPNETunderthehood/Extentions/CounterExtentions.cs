using ASPNETunderthehood.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETunderthehood.Extentions
{
    public static class CounterExtentions
    {
        public static IApplicationBuilder UseCounter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CounterMiddleware>();
        }
    }
}
