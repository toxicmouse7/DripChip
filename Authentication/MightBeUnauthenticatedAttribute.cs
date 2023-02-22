
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DripChip.Authentication;

public class MightBeUnauthenticatedAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.Request.Headers.Authorization.Any() &&
            !context.HttpContext.User.Identity!.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}