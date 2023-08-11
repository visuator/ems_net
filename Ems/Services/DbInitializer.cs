﻿using Ems.Infrastructure.Storages;
using Microsoft.EntityFrameworkCore;

namespace Ems.Services;

public class DbInitializer : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DbInitializer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken token)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EmsDbContext>();

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(token);
        if (pendingMigrations.Any()) await dbContext.Database.MigrateAsync(token);
    }

    public Task StopAsync(CancellationToken token)
    {
        return Task.CompletedTask;
    }
}