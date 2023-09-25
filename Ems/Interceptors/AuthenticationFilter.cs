using System.Security.Claims;
using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ems.Interceptors;

public class AuthenticationFilter : IAsyncAuthorizationFilter
{
    private readonly IAuthStorage _authStorage;

    public AuthenticationFilter(IAuthStorage authStorage)
    {
        _authStorage = authStorage;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var roles = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role)
            .Select(x => Enum.Parse<Role>(x.Value, true)).ToList();
        _authStorage.CurrentRoles = roles;
        return Task.CompletedTask;
    }
}