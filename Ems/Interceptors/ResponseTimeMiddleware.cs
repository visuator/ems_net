﻿using System.Diagnostics;

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
            httpContext.Response.Headers.Add("X-Response-Time",
                new[] { watch.ElapsedMilliseconds.ToString() });

            watch.Stop();
            return Task.CompletedTask;
        }, context);

        await next(context);
    }
}