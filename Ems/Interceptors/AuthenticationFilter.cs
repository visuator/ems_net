using Ems.Core.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Ems.Interceptors;

public class AuthenticationFilter : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var roles = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role)
            .Select(x => Enum.Parse<Role>(x.Value, true)).ToList();
        return Task.CompletedTask;
    }
}