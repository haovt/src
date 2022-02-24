using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MusicStore.Middlewares
{
    public class MyMiddleware2
    {
        private readonly RequestDelegate _next;

        public MyMiddleware2(RequestDelegate next)
        {
            _next = next;
        }

        // /Store/Details/55555
        public async Task InvokeAsync(HttpContext context)
        {
            // TODO

            await context.Response.WriteAsync("Custom Async exception middleware");
            //await _next(context);
        }

    }
}
