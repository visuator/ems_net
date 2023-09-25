using System.Linq.Expressions;
using System.Reflection;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Ems.Domain.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> PathInclude<T>(this IQueryable<T> query) where T : class
    {
        List<MemberExpression> FindNavigationPaths(Expression e)
        {
            var properties = new List<MemberExpression>();

            if (e is MethodCallExpression mce)
            {
                switch (mce.Method.Name)
                {
                    case "Where":
                        properties.AddRange(FindNavigationPaths(mce.Arguments[1]));
                        break;
                    case "Select":
                        properties.AddRange(FindNavigationPaths(mce.Arguments[0]));
                        properties.AddRange(FindNavigationPaths(mce.Arguments[1]));
                        break;
                    default:
                        properties.AddRange(FindNavigationPaths(mce.Arguments[0]));
                        break;
                }
            }
            else if (e is UnaryExpression { Operand: LambdaExpression le })
            {
                if (le.Body is MethodCallExpression mce1) properties.AddRange(FindNavigationPaths(mce1));
            }
            else if (e is LambdaExpression le1)
            {
                if (le1.Body is MemberExpression me)
                    properties.AddRange(FindNavigationPaths(me));
            }
            else if (e is MemberExpression { Member.MemberType: MemberTypes.Property } me)
            {
                properties.Add(me);
            }

            return properties;
        }

        List<MemberExpression> FindIncludes(Expression e)
        {
            var properties = new List<MemberExpression>();

            if (e is MethodCallExpression mce)
            {
                switch (mce.Method.Name)
                {
                    case "Include":
                        properties.AddRange(FindIncludes(mce.Arguments[1]));
                        break;
                    case "ThenInclude":
                        properties.AddRange(FindIncludes(mce.Arguments[0]));
                        properties.AddRange(FindIncludes(mce.Arguments[1]));
                        break;
                    default:
                        properties.AddRange(FindIncludes(mce.Arguments[0]));
                        break;
                }
            }
            else if (e is UnaryExpression { Operand: LambdaExpression le })
            {
                if (le.Body is MemberExpression me) properties.AddRange(FindNavigationPaths(me));
            }
            else if (e is MemberExpression { Member.MemberType: MemberTypes.Property } me)
            {
                properties.Add(me);
            }

            return properties;
        }

        var navigationPaths = FindNavigationPaths(query.Expression);
        var includePaths = FindIncludes(query.Expression);

        List<PropertyInfo> TraverseMember(MemberExpression e)
        {
            var properties = new List<PropertyInfo>();

            if (e.Member.MemberType == MemberTypes.Property)
                if (e.Member is PropertyInfo pi)
                {
                    if (pi.PropertyType.IsClass || pi.PropertyType.GetInterfaces().Where(x => x.IsGenericType)
                            .Any(x => x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                        properties.Add(pi);

                    if (e.Expression is not null)
                        if (e.Expression is MemberExpression me1)
                            properties.AddRange(TraverseMember(me1));
                }

            return properties;
        }

        List<string> GetIncludePath(MemberExpression e)
        {
            var accumulator = new List<string>();
            if (e.Expression is MemberExpression e1)
                accumulator.AddRange(GetIncludePath(e1));
            accumulator.Add(e.Member.Name);

            return accumulator;
        }

        var ip = includePaths.SelectMany(TraverseMember).ToList();
        var except = navigationPaths.Where(x =>
        {
            var np = TraverseMember(x);
            if (np.Count == 0) return false;
            return !ip.Any(ix => np.Any(y => y.PropertyType == ix.PropertyType && y.DeclaringType == ix.DeclaringType));
        }).ToList();

        foreach (var pi in except)
        {
            var includePath = GetIncludePath(pi);
            query.Include(string.Join('.', includePath));
        }

        return query;
    }

    public static IQueryable<Class> ResolveByAccountRoles(this IQueryable<Class> query, Guid accountId,
        List<Role> roles)
    {
        if (roles.Contains(Role.Student))
            return query.Where(x => x.Template!.Group!.Students.Select(s => s.Id).Contains(accountId))
                .PathInclude();

        // аналогично, только тут уже Lecturer -> Account -> AccountRoles
        return roles.Contains(Role.Lecturer) ? query.Where(x => x.Lecturer.Account.Id == accountId) : query;
    }

    public static IQueryable<T> ODataMapFromRoles<T>(this IQueryable<T> query, List<Role> roles)
    {
        //todo: refactor
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
                if (expr is null) continue;
                var args = expr.Arguments[0] as MethodCallExpression;
                if (args is null) continue;
                if (args.Method.Name != "Select") continue;

                var l1 = args.Arguments[1] as LambdaExpression;
                var memInit = l1.Body as MemberInitExpression;
                var t = FilteredBindings(roles, memInit);
                memberBindings = memberBindings.Where(x => x != fb).ToList();
                memberBindings.Add(Expression.Bind(fb.Member,
                    Expression.Call(expr.Method,
                        Expression.Call(args.Method, args.Arguments[0], Expression.Lambda(t, false, l1.Parameters)))));
            }

            return Expression.MemberInit(memberInitExpression.NewExpression, memberBindings);
        }

        var lambda = Expression.Lambda(FilteredBindings(roles, dt4), false, dt3.Parameters);
        var call = Expression.Call(null, dt1.Method, dt1.Arguments[0], lambda);

        return query.Provider.CreateQuery<T>(call);
    }
}