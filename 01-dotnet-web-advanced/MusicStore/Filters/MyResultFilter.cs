using Microsoft.AspNetCore.Mvc.Filters;

namespace MusicStore.Filters
{
    public class MyResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            context.Canceled = true;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // before
        }
    }
}
