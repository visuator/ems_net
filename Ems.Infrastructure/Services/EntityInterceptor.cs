using Ems.Core.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ems.Infrastructure.Services;

public class EntityInterceptor : SaveChangesInterceptor
{
    private static void ModifyEntries(DbContext context)
    {
        var entries = context.ChangeTracker.Entries<EntityBase>();
        var now = DateTimeOffset.UtcNow;
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Property(x => x.CreatedAt).CurrentValue = now;
            entry.Property(x => x.UpdatedAt).CurrentValue = now;
        }
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);
        ModifyEntries(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);
        ModifyEntries(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}