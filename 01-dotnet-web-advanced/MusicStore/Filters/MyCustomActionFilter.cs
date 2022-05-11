using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MusicStore.Filters
{
    public class MyCustomActionFilter : IActionFilter
    {
        private readonly ILogger<AuditTrailsActionFilter> _logger;

        public MyCustomActionFilter(ILogger<AuditTrailsActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //context.Canceled = true;
            //context.Result;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //context.Result = NotFound();
            _logger.LogInformation($"MyCustomActionFilter: {context.HttpContext.Request.Method} {context.ActionDescriptor.DisplayName}");
        }

        //public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
