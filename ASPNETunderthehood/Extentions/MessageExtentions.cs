using ASPNETunderthehood.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETunderthehood.Extentions
{
    public static class MessageExtentions
    {
        public static IApplicationBuilder UseMessage(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MessageMiddleware>();
        }
    }
}
