using Ems.Domain.Services;
using Ems.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ems.Interceptors;

public class CurrentClassBindingActionFilter : IAsyncActionFilter
{
    private readonly IClassService _classService;

    public CurrentClassBindingActionFilter(IClassService classService)
    {
        _classService = classService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.Values.Where(x => x is IAuthenticated).Where(x => x is IRequestTimeStamp).Where(x => x is ICurrentClassBinding).Any())
        {
            await next();
            return;
        }
        
        // Просто чтобы не вводить 3й интерфейс и не мешать их работе друг с другом
        var models = context.ActionArguments.Values.Where(x => x is IAuthenticated).Where(x => x is IRequestTimeStamp).Where(x => x is ICurrentClassBinding).ToList();
        foreach (var model in models)
        {
            var timeStamp = model as IRequestTimeStamp;
            var authenticated = model as IAuthenticated;
            var currentClassBinding = model as ICurrentClassBinding;

            currentClassBinding!.CurrentClass = await _classService.GetCurrent(authenticated!.AccountId, timeStamp!.RequestedAt);
        }
    }
}