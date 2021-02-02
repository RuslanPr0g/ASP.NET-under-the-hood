using ASPNETunderthehood.Services.Counter;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETunderthehood.Middlewares
{
    public class CounterMiddleware
    {
        private readonly RequestDelegate _next;
        private int countRequests = 0;

        public CounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ICounter counter, CounterService counterService)
        {
            countRequests++;
            httpContext.Response.ContentType = "text/html;charset=utf-8";
            await httpContext.Response.WriteAsync($"Запрос {countRequests}; Counter: {counter.Value}; Service: {counterService.Counter.Value}");
        }
    }
}
