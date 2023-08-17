using Ems.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ems.Interceptors;

public class RequestTimeStampActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.Values.OfType<IRequestTimeStamp>().Any())
        {
            await next();
            return;
        }

        var now = DateTime.UtcNow;
        var models = context.ActionArguments.Values.OfType<IRequestTimeStamp>().ToList();
        foreach (var model in models) model.RequestedAt = now;
        await next();
    }
}