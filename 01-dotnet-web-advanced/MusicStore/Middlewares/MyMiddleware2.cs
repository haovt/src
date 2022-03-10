using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MusicStore.Filters;
using System;
using System.Threading.Tasks;

namespace MusicStore.Middlewares
{
    public class MyMiddleware2
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MyMiddleware2> _logger;


        // Q: DI for custom service to check auth
        public MyMiddleware2(RequestDelegate next, ILogger<MyMiddleware2> logger)
        {
            _next = next;
            _logger = logger;
        }

        // /Store/Details/55555
        public async Task InvokeAsync(HttpContext context)
        {
            // TODO

            //await context.Response.WriteAsync("Custom Async exception middleware");

            //await Task.CompletedTask();
            _logger.LogInformation("Test");
            try
            {
                await _next(context);
            }
            catch (Exception ex)       {
                //sdsd

                throw;
            }
     
        }
    }
}
