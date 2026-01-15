using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace device_registry;

public class AuthorizeFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var userContext = httpContext.RequestServices.GetRequiredService<CurrentUser>();
        
        if (httpContext.Request.Headers.TryGetValue("user-id", out var userIdValues) &&
            !string.IsNullOrEmpty(userIdValues.FirstOrDefault()))
        {
            var userId = userIdValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(userId))
            {
                userContext.Id = userId;
                return;
            }
        }
        context.Result = new UnauthorizedResult();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}