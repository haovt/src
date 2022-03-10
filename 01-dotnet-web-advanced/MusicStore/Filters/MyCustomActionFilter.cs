using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

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
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"MyCustomActionFilter: {context.HttpContext.Request.Method} {context.ActionDescriptor.DisplayName}");
        }
    }
}
