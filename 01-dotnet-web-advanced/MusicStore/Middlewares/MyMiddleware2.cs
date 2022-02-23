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

        // "/Store/Details/437"
        public async Task Invoke(HttpContext context)
        {
            // TODO

            await _next(context);
            //return _next(context);
        }
    }
}
