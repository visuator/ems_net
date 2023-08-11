using System.Collections;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Domain.Services;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace Ems.Tests;

[TestFixture]
public class AccountServiceTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        services.AddDbContext<EmsDbContext>(opt => { opt.UseInMemoryDatabase("test_db"); });
        services.AddScoped<AccountService>();
        var jwtServiceMock = Substitute.For<IJwtService>();
        services.AddScoped(x => jwtServiceMock);
        services.AddOptions();
        services.Configure<AccountOptions>(x =>
        {
            x.AllowedFailedLoginAttempts = 3;
            x.LinkExpirationTime = TimeSpan.FromMinutes(1);
            x.PasswordLength = 16;
            x.LockExpirationTime = TimeSpan.FromSeconds(1);
        });
        services.AddScoped<IPasswordProvider, PasswordProvider>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [TearDown]
    public async Task TearDown()
    {
        var service = _serviceProvider.GetRequiredService<EmsDbContext>();

        var accountsToRemove = await service.Accounts.AsTracking().ToListAsync();
        service.Accounts.RemoveRange(accountsToRemove);
        var refreshTokensToRemove = await service.RefreshTokens.AsTracking().ToListAsync();
        service.RefreshTokens.RemoveRange(refreshTokensToRemove);

        await service.SaveChangesAsync();
    }

    private IServiceProvider _serviceProvider;

    [TestCaseSource(typeof(Data), nameof(Data.ExistsTestCases))]
    public async Task ExistsTest(Account account)
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        var actual = await service.Exists(account.Email);

        Assert.That(actual, Is.True);
    }

    [TestCaseSource(typeof(Data), nameof(Data.IsConfirmedTestCases))]
    public async Task IsConfirmedTest(Account account, bool expected)
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        var actual = await service.IsConfirmed(account.Email);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public async Task CheckPasswordTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();
        var accountOptions = _serviceProvider.GetRequiredService<IOptions<AccountOptions>>().Value;

        var requestedAt = DateTimeOffset.UtcNow;
        var password = "test_password";
        var model = HashHelper.HashPassword(password);
        var account = new Account
        {
            Email = "test@email.ru",
            Phone = "test",
            PasswordHash = model.PasswordHash,
            PasswordSalt = model.PasswordSalt
        };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        var actual = await service.CheckPassword(account.Email, password, requestedAt);
        Assert.That(actual, Is.True);
        var actualAccount = await dbContext.Accounts.Where(x => x.Id == account.Id).SingleAsync();
        Assert.That(actualAccount.LockExpiresAt, Is.Null);
        Assert.That(actualAccount.FailedAttempts, Is.EqualTo(0));

        for (var i = 0; i <= accountOptions.AllowedFailedLoginAttempts; i++)
        {
            actual = await service.CheckPassword(account.Email, "invalid_password", requestedAt);
            Assert.That(actual, Is.False);
        }

        actualAccount = await dbContext.Accounts.Where(x => x.Id == account.Id).SingleAsync();
        Assert.That(actualAccount.LockExpiresAt, Is.EqualTo(requestedAt.Add(accountOptions.LockExpirationTime)));
        Assert.That(actual, Is.False);
    }

    [Test]
    public async Task IsLockedTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();
        var accountOptions = _serviceProvider.GetRequiredService<IOptions<AccountOptions>>().Value;
        var now = DateTimeOffset.UtcNow;
        var lockExpiration = now.Add(accountOptions.LockExpirationTime);
        var account = new Account
        {
            Email = "test@email.ru",
            Phone = "test",
            PasswordHash = "test",
            PasswordSalt = "test",
            LockExpiresAt = lockExpiration
        };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        var actual = await service.IsLocked(account.Email, now);
        Assert.That(actual, Is.True);
        actual = await service.IsLocked(account.Email, lockExpiration.AddSeconds(1));
        Assert.That(actual, Is.False);
    }

    [TestCaseSource(typeof(Data), nameof(Data.IsRevokedTestCases))]
    public async Task IsRevokedTest(RefreshToken refreshToken, bool expected)
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();

        await dbContext.RefreshTokens.AddAsync(refreshToken);
        await dbContext.SaveChangesAsync();
        var actual = await service.IsRevoked(refreshToken.Value);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public async Task RevokeSessionTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();
        var refreshToken = new RefreshToken
        {
            Account = new Account
            {
                Email = "test@email.ru",
                Phone = "test",
                PasswordHash = "test",
                PasswordSalt = "test"
            },
            Value = "initialSessionToken"
        };

        await dbContext.RefreshTokens.AddAsync(refreshToken);
        await dbContext.SaveChangesAsync();
        await service.RevokeSession(new RevokeSessionModel { AccountId = refreshToken.AccountId });

        var actual = await dbContext.RefreshTokens.Where(x => x.Id == refreshToken.Id).SingleAsync();
        Assert.That(actual.RevokedAt, Is.Not.Null);
    }

    [Test]
    public async Task LoginTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();
        var jwtService = _serviceProvider.GetRequiredService<IJwtService>();
        jwtService.GetJwt(Arg.Any<Account>()).Returns(new JwtModel { AccessToken = "test" });

        var password = "test_password";
        var model = HashHelper.HashPassword(password);
        var account = new Account
        {
            Email = "test@email.ru",
            Phone = "test",
            PasswordHash = model.PasswordHash,
            PasswordSalt = model.PasswordSalt,
            Roles = new List<AccountRole>
            {
                new() { Role = Role.Admin }
            }
        };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        var actual = await service.Login(new LoginModel
            { Email = "test@email.ru", Password = password, RequestedAt = DateTimeOffset.UtcNow });
        var refreshToken = await dbContext.RefreshTokens.Where(x => x.AccountId == account.Id).SingleOrDefaultAsync();
        Assert.That(actual.RefreshToken, Is.Not.Null);
        Assert.That(actual.Roles, Is.EquivalentTo(new[] { Role.Admin }));
        Assert.That(actual.AccessToken, Is.EqualTo("test"));
        Assert.That(refreshToken, Is.Not.Null);
    }

    [Test]
    public async Task LoginOAuthTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();
        var jwtService = _serviceProvider.GetRequiredService<IJwtService>();
        jwtService.GetJwt(Arg.Any<Account>()).Returns(new JwtModel { AccessToken = "test" });

        var password = "test_password";
        var model = HashHelper.HashPassword(password);
        var account = new Account
        {
            Email = "test@email.ru",
            Phone = "test",
            PasswordHash = model.PasswordHash,
            PasswordSalt = model.PasswordSalt,
            Roles = new List<AccountRole>
            {
                new() { Role = Role.Admin }
            }
        };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        var actual = await service.Login(new OAuthLoginModel { ExternalEmail = "test@email.ru" });
        var refreshToken = await dbContext.RefreshTokens.Where(x => x.AccountId == account.Id).SingleOrDefaultAsync();
        Assert.That(actual.RefreshToken, Is.Not.Null);
        Assert.That(actual.Roles, Is.EquivalentTo(new[] { Role.Admin }));
        Assert.That(actual.AccessToken, Is.EqualTo("test"));
        Assert.That(refreshToken, Is.Not.Null);
    }

    [Test]
    public async Task RefreshTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();
        var jwtService = _serviceProvider.GetRequiredService<IJwtService>();
        jwtService.GetJwt(Arg.Any<Account>()).Returns(new JwtModel { AccessToken = "test" });

        var refreshToken = new RefreshToken
        {
            Account = new Account
            {
                Email = "test@email.ru",
                Phone = "test",
                PasswordHash = "test",
                PasswordSalt = "test",
                Roles = new List<AccountRole>
                {
                    new() { Role = Role.Admin }
                }
            },
            Value = "initialSessionToken"
        };

        await dbContext.RefreshTokens.AddAsync(refreshToken);
        await dbContext.SaveChangesAsync();

        var actual = await service.Refresh(new RefreshModel { RefreshToken = "initialSessionToken" });
        Assert.Multiple(() =>
        {
            Assert.That(actual.RefreshToken, Is.Not.Null);
            Assert.That(actual.Roles, Is.EquivalentTo(new[] { Role.Admin }));
            Assert.That(actual.AccessToken, Is.EqualTo("test"));
        });

        actual = await service.Refresh(new RefreshModel { RefreshToken = actual.RefreshToken });
        var actualRefreshToken = await dbContext.RefreshTokens.Where(x => x.Value == actual.RefreshToken).SingleAsync();
        Assert.That(actualRefreshToken.SessionTokenId, Is.EqualTo(refreshToken.Id));
    }

    [Test]
    public async Task ReconfirmTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();

        var now = DateTimeOffset.UtcNow;
        var account = new Account
        {
            Email = "test@email.ru",
            ConfirmationExpiresAt = now,
            ConfirmationToken = "test",
            PasswordHash = "",
            PasswordSalt = "",
            Phone = "",
            ConfirmedAt = null
        };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        await service.Reconfirm(new ReconfirmModel
        {
            Email = "test@email.ru",
            RequestedAt = now
        });
        var actualAccount = await dbContext.Accounts.Where(x => x.Id == account.Id).SingleAsync();
        var actualEmail = await dbContext.Emails.OfType<ReconfirmationEmail>().Where(x => x.Recipient == account.Email)
            .SingleAsync();

        Assert.That(actualAccount.ConfirmationToken, Is.Not.EqualTo("test"));
        Assert.That(actualAccount.ConfirmedAt, Is.Null);
        Assert.That(actualAccount.ConfirmationExpiresAt, Is.Not.EqualTo(now));

        Assert.That(actualEmail.Recipient, Is.EqualTo(account.Email));
        Assert.That(actualEmail.ConfirmationToken, Is.EqualTo(actualAccount.ConfirmationToken));
        Assert.That(actualEmail.ConfirmationExpiresAt, Is.EqualTo(actualAccount.ConfirmationExpiresAt));
    }

    [Test]
    public async Task ResetPasswordTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();

        var now = DateTimeOffset.UtcNow;
        var account = new Account
        {
            Email = "test@email.ru",
            PasswordResetExpiresAt = now,
            PasswordResetToken = "test",
            PasswordHash = "",
            PasswordSalt = "",
            Phone = "",
            ConfirmedAt = null
        };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        await service.ResetPassword(new ResetPasswordModel
        {
            Email = "test@email.ru",
            RequestedAt = now
        });
        var actualAccount = await dbContext.Accounts.Where(x => x.Id == account.Id).SingleAsync();
        var actualEmail = await dbContext.Emails.OfType<PasswordResetEmail>().Where(x => x.Recipient == account.Email)
            .SingleAsync();

        Assert.That(actualAccount.ConfirmationToken, Is.Not.EqualTo("test"));
        Assert.That(actualAccount.ConfirmedAt, Is.Null);
        Assert.That(actualAccount.ConfirmationExpiresAt, Is.Not.EqualTo(now));
        Assert.That(actualAccount.PasswordHash, Is.EqualTo(""));
        Assert.That(actualAccount.PasswordSalt, Is.EqualTo(""));

        Assert.That(actualEmail.Recipient, Is.EqualTo(account.Email));
        Assert.That(actualEmail.PasswordResetToken, Is.EqualTo(actualAccount.PasswordResetToken));
        Assert.That(actualEmail.PasswordResetExpiresAt, Is.EqualTo(actualAccount.PasswordResetExpiresAt));
    }

    [Test]
    public async Task ConfirmTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();

        var now = DateTimeOffset.UtcNow;
        var account = new Account
        {
            Email = "test@email.ru",
            ConfirmationToken = "test",
            PasswordHash = "",
            PasswordSalt = "",
            Phone = "",
            ConfirmedAt = null
        };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        await service.Confirm(new ConfirmModel { ConfirmationToken = "test", RequestedAt = now });
        var actualAccount = await dbContext.Accounts.Where(x => x.Id == account.Id).SingleAsync();

        Assert.That(actualAccount.ConfirmationToken, Is.Null);
        Assert.That(actualAccount.ConfirmationExpiresAt, Is.Null);
        Assert.That(actualAccount.ConfirmedAt, Is.EqualTo(now));
    }

    [Test]
    public async Task ConfirmPasswordResetTest()
    {
        var service = _serviceProvider.GetRequiredService<AccountService>();
        var dbContext = _serviceProvider.GetRequiredService<EmsDbContext>();

        var now = DateTimeOffset.UtcNow;
        var account = new Account
        {
            Email = "test@email.ru",
            PasswordResetToken = "test",
            PasswordHash = "",
            PasswordSalt = "",
            Phone = "",
            ConfirmedAt = null
        };

        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();

        await service.ConfirmPasswordReset(new ConfirmPasswordResetModel
        {
            PasswordResetToken = "test",
            RequestedAt = now
        });
        var actualAccount = await dbContext.Accounts.Where(x => x.Id == account.Id).SingleAsync();

        Assert.That(actualAccount.PasswordHash, Is.Not.EqualTo(""));
        Assert.That(actualAccount.PasswordSalt, Is.Not.EqualTo(""));
    }

    private class Data
    {
        public static IEnumerable ExistsTestCases()
        {
            yield return new object[]
            {
                new Account { Email = "test@email.ru", Phone = "test", PasswordHash = "test", PasswordSalt = "test" }
            };
        }

        public static IEnumerable IsConfirmedTestCases()
        {
            yield return new object[]
            {
                new Account { Email = "test@email.ru", Phone = "test", PasswordHash = "test", PasswordSalt = "test" },
                false
            };
            yield return new object[]
            {
                new Account
                {
                    Email = "test@email.ru", Phone = "test", PasswordHash = "test", PasswordSalt = "test",
                    ConfirmedAt = DateTimeOffset.UtcNow
                },
                true
            };
        }

        public static IEnumerable IsRevokedTestCases()
        {
            yield return new object[]
            {
                new RefreshToken
                {
                    Account = new Account
                    {
                        Email = "test",
                        Phone = "test",
                        PasswordSalt = "test",
                        PasswordHash = "test"
                    },
                    RevokedAt = DateTimeOffset.UtcNow,
                    Value = "test"
                },
                true
            };
            yield return new object[]
            {
                new RefreshToken
                {
                    Account = new Account
                    {
                        Email = "test",
                        Phone = "test",
                        PasswordSalt = "test",
                        PasswordHash = "test"
                    },
                    Value = "test"
                },
                false
            };
        }
    }
}