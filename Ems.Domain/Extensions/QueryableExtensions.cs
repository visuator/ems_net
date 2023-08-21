using Ems.Core.Entities;
using Ems.Core.Entities.Enums;

namespace Ems.Domain.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Class> ResolveByAccountRoles(this IQueryable<Class> query, List<Role> roles, Guid accountId)
    {
        // auto-include
        if (roles.Contains(Role.Student))
            return query.Where(x => x.Template!.Group!.Students.Select(s => s.Id).Contains(accountId));
        return roles.Contains(Role.Lecturer) ? query.Where(x => x.LecturerId == accountId) : query;
    }
}