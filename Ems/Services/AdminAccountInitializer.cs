using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ems.Services;

public class AdminAccountInitializer : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AdminAccountInitializer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken token)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EmsDbContext>();
        var adminAccountOptions = scope.ServiceProvider.GetRequiredService<IOptions<AdminAccountOptions>>().Value;

        var accountExists = await dbContext.Accounts.Where(x => x.Email == adminAccountOptions.Email).AnyAsync(token);
        if (accountExists) return;

        var passwordModel = HashHelper.HashPassword(adminAccountOptions.Password);
        var account = new Account
        {
            Email = adminAccountOptions.Email,
            PasswordHash = passwordModel.PasswordHash,
            PasswordSalt = passwordModel.PasswordSalt,
            ConfirmedAt = DateTime.UtcNow,
            Phone = adminAccountOptions.Phone,
            Roles = new List<AccountRole>
            {
                new() { Role = Role.Admin }
            }
        };

        await dbContext.Accounts.AddAsync(account, token);
        await dbContext.SaveChangesAsync(token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}