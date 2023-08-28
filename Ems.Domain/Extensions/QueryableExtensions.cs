using System.Linq.Expressions;
using System.Reflection;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Ems.Domain.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Class> ResolveByAccountRoles(this IQueryable<Class> query, Guid accountId, List<Role> roles)
    {
        // auto-include
        if (roles.Contains(Role.Student))
            return query.Where(x => x.Template!.Group!.Students.Select(s => s.Id).Contains(accountId));
        return roles.Contains(Role.Lecturer) ? query.Where(x => x.LecturerId == accountId) : query;
    }

    public static IQueryable<T> ODataMapFromRoles<T>(this IQueryable<T> query, List<Role> roles)
    {
        var dt1 = query.Expression as MethodCallExpression;
        var dt2 = dt1!.Arguments[1] as UnaryExpression;
        var dt3 = dt2!.Operand as LambdaExpression;
        var dt4 = dt3!.Body as MemberInitExpression;

        MemberInitExpression FilteredBindings(List<Role> list, MemberInitExpression? memberInitExpression)
        {
            var props = memberInitExpression!.Type.GetRuntimeProperties().Where(x =>
            {
                var a = x.GetCustomAttribute<NotMapWhenInRoleAttribute>();
                return a is not null && list.Contains(a.Role);
            }).ToList();
            var memberBindings = memberInitExpression.Bindings.Where(x =>
                    x.Member.MemberType == MemberTypes.Property &&
                    props.All(y => y.PropertyType != (x.Member as PropertyInfo).PropertyType))
                .ToList();
            foreach (var fb in memberBindings)
            {
                if (fb is not MemberAssignment fba) continue;
                var expr = fba.Expression as MethodCallExpression;
                if(expr is null) continue;
                var args = expr.Arguments[0] as MethodCallExpression;
                if (args is null) continue;
                if (args.Method.Name != "Select") continue;
                
                var l1 = args.Arguments[1] as LambdaExpression;
                var memInit = l1.Body as MemberInitExpression;
                var t = FilteredBindings(roles, memInit);
                memberBindings = memberBindings.Where(x => x != fb).ToList();
                memberBindings.Add(Expression.Bind(fb.Member,
                    Expression.Call(expr.Method, Expression.Call(args.Method, args.Arguments[0], Expression.Lambda(t, false, l1.Parameters)))));
            }

            return Expression.MemberInit(memberInitExpression.NewExpression, memberBindings);
        }
        
        var lambda = Expression.Lambda(FilteredBindings(roles, dt4), false, dt3.Parameters);
        var call = Expression.Call(null, dt1.Method, dt1.Arguments[0], lambda);

        return query.Provider.CreateQuery<T>(call);
    }
}