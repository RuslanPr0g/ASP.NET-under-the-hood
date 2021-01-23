using ASPNETunderthehood.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ASPNETunderthehood.Extentions
{
    public static class TokenExtensions
    {
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}
