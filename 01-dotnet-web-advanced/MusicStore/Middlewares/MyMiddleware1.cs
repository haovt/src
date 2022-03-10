using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace MusicStore.Middlewares
{
    public class MyMiddleware1
    {
        private readonly RequestDelegate _next;

        public MyMiddleware1(RequestDelegate next)
        {
            _next = next;
        }


        public Task Invoke(HttpContext context)
        {
            // TODO

            // return context.Response.WriteAsync("Custom Sync exception middleware");
            try
            {
                return _next(context);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
