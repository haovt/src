using Microsoft.AspNetCore.Http;
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

        public async Task Invoke(HttpContext context)
        {
            // TODO


            await _next(context);
        }
    }
}
