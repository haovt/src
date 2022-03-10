using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MusicStore.Models;

namespace MusicStore.Filters
{
    public class AuditTrailsActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            ActionLog log = new ActionLog()
            {
                Controller = filterContext.ActionDescriptor.DisplayName,
                Action = string.Format("TRAIL_FILTER {0} {1}" , filterContext.HttpContext.Request.Method, filterContext.ActionDescriptor.DisplayName)
            };
        }
    }
}
