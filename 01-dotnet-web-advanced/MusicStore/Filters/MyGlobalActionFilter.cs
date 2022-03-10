using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MusicStore.Filters
{
    public class MyGlobalActionFilter : IActionFilter
    {
        private readonly ILogger<AuditTrailsActionFilter> _logger;

        public MyGlobalActionFilter(ILogger<AuditTrailsActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Log by GLOBAL filter: {context.HttpContext.Request.Method} {context.ActionDescriptor.DisplayName}");
        }
    }
}
