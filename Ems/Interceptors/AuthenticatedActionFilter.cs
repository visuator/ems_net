using System.Security.Claims;
using Ems.Domain.Constants;
using Ems.Infrastructure.Exceptions;
using Ems.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ems.Interceptors;

public class AuthenticatedActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var accountClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
        if (accountClaim is not null)
        {
            var models = context.ActionArguments.Values.OfType<IAuthenticated>().ToList();
            foreach (var model in models) model.AccountId = Guid.Parse(accountClaim.Value!);
        }
        else
        {
            throw new AuthenticationException(ErrorMessages.System.InvalidJwt);
        }

        await next();
    }
}