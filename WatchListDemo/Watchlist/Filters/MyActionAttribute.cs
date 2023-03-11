using Microsoft.AspNetCore.Mvc.Filters;
using Watchlist.Contracts;

namespace Watchlist.Filters
{
    public class MyActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var movieService = context.HttpContext.RequestServices.GetService<IMovieService>();
            movieService.GetGenresAsync();

            base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
