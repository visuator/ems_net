using System.Diagnostics;
using Ems.Constants;

namespace Ems.Interceptors;

public class ResponseTimeMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var watch = new Stopwatch();
        watch.Start();

        context.Response.OnStarting(state =>
        {
            var httpContext = (HttpContext)state;
            httpContext.Response.Headers.Add(CustomHeaderNames.ResponseTime,
                new[] { watch.ElapsedMilliseconds.ToString() });

            watch.Stop();
            return Task.CompletedTask;
        }, context);

        await next(context);
    }
}