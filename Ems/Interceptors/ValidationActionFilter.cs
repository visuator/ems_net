using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ems.Interceptors;

public class ValidationActionFilter<T> : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationActionFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator is null) return;
        var validatable = context.ActionArguments.Values.OfType<T>().FirstOrDefault();
        if (validatable is null) return;
        var result = await validator.ValidateAsync(validatable);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
            return;
        }

        await next();
    }
}