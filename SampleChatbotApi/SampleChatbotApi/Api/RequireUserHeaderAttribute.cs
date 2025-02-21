using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleChatbotApi.Api;

internal class RequireUserHeaderAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Request.Headers.TryGetValue(CustomHeaders.Username, out var usernameHeader);

        if (string.IsNullOrEmpty(usernameHeader))
        {
            context.HttpContext.Response.StatusCode = 401;
            await context.HttpContext.Response.CompleteAsync();
            return;
        }

        var identity = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, usernameHeader!)
        ]);

        context.HttpContext.User = new ClaimsPrincipal(identity);

        await next();
    }
}