using AutoMapper;
using AutoMapper.AspNet.OData;
using AutoMapper.QueryableExtensions;
using Ems.Core.Entities;
using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Options;
using Ems.Infrastructure.Services;
using Ems.Infrastructure.Storages;
using Ems.Models;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ems.Domain.Services;

public class AccountService : IAccountService
{
    private readonly AccountOptions _accountOptions;
    private readonly EmsDbContext _dbContext;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly IPasswordProvider _passwordProvider;

    public AccountService(EmsDbContext dbContext, IJwtService jwtService, IOptions<AccountOptions> accountOptions,
        IPasswordProvider passwordProvider, IMapper mapper)
    {
        _dbContext = dbContext;
        _jwtService = jwtService;
        _passwordProvider = passwordProvider;
        _mapper = mapper;
        _accountOptions = accountOptions.Value;
    }

    public async Task<bool> Exists(string email, CancellationToken token = new())
    {
        return await _dbContext.Accounts.Where(x => x.Email == email).AnyAsync(token);
    }

    public async Task<bool> Exists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.Accounts.Where(x => x.Id == id).AnyAsync(token);
    }

    public async Task<bool> IsConfirmed(string email, CancellationToken token = new())
    {
        var dbAccount = await _dbContext.Accounts.Where(x => x.Email == email).Select(x => new { x.ConfirmedAt })
            .SingleAsync(token);
        return dbAccount.ConfirmedAt is not null;
    }

    public async Task<bool> IsConfirmed(Guid id, CancellationToken token = new())
    {
        var account = await _dbContext.Accounts.Where(x => x.Id == id).Select(x => new { x.ConfirmedAt })
            .SingleAsync(token);
        return account.ConfirmedAt is not null;
    }

    public async Task<bool> AnyByConfirmationToken(string confirmationToken, CancellationToken token = new())
    {
        return await _dbContext.Accounts.Where(x => x.ConfirmationToken == confirmationToken).AnyAsync(token);
    }

    public async Task<bool> AnyByPasswordResetToken(string passwordResetToken, CancellationToken token = new())
    {
        return await _dbContext.Accounts.Where(x => x.PasswordResetToken == passwordResetToken).AnyAsync(token);
    }

    public async Task<bool> CheckPasswordResetExpiration(string passwordResetToken, DateTimeOffset requestedAt,
        CancellationToken token = new())
    {
        return await _dbContext.Accounts.Where(x => x.PasswordResetToken == passwordResetToken)
            .Select(x => x.PasswordResetExpiresAt).SingleAsync(token) <= requestedAt;
    }

    public async Task<bool> CheckPassword(string email, string password, DateTimeOffset requestedAt,
        CancellationToken token = new())
    {
        var dbAccount = await _dbContext.Accounts.AsTracking().Where(x => x.Email == email).SingleAsync(token);
        var incomingHash = HashHelper.HashPassword(password, dbAccount.PasswordSalt).PasswordHash;
        var passwordEqual = incomingHash.SequenceEqual(dbAccount.PasswordHash);
        if (passwordEqual)
        {
            dbAccount.FailedAttempts = 0;
            dbAccount.LockExpiresAt = null;
            await _dbContext.SaveChangesAsync(token);
            return true;
        }

        if (dbAccount.FailedAttempts >= _accountOptions.AllowedFailedLoginAttempts)
        {
            dbAccount.LockExpiresAt = requestedAt.Add(_accountOptions.LockExpirationTime);
            await _dbContext.SaveChangesAsync(token);
            return false;
        }

        dbAccount.FailedAttempts += 1;
        await _dbContext.SaveChangesAsync(token);
        return false;
    }

    public async Task<bool> CheckConfirmationExpiration(string confirmationToken, DateTimeOffset requestedAt,
        CancellationToken token = new())
    {
        return await _dbContext.Accounts.Where(x => x.ConfirmationToken == confirmationToken)
            .Select(x => x.ConfirmationExpiresAt).SingleAsync(token) <= requestedAt;
    }


    public async Task<bool> IsLocked(string email, DateTimeOffset requestedAt, CancellationToken token = new())
    {
        var dbAccount = await _dbContext.Accounts.Where(x => x.Email == email).Select(x => new { x.LockExpiresAt })
            .SingleAsync(token);
        return dbAccount.LockExpiresAt is not null && requestedAt <= dbAccount.LockExpiresAt;
    }

    public async Task<bool> IsRevoked(string refreshToken, CancellationToken token = new())
    {
        return await _dbContext.RefreshTokens.Include(x => x.SessionToken)
            .Where(x => x.Value == refreshToken).Select(x => x.SessionToken == null ? null : x.SessionToken.RevokedAt)
            .SingleAsync(token) is not null;
    }

    public async Task<bool> ExternalAccountExists(Guid id, CancellationToken token = new())
    {
        return await _dbContext.ExternalAccounts.Where(x => x.Id == id).AnyAsync(token);
    }

    public async Task<bool> ExternalAccountExists(string externalEmail, CancellationToken token = new())
    {
        return await _dbContext.ExternalAccounts.Where(x => x.ExternalEmail == externalEmail).AnyAsync(token);
    }

    public async Task RevokeSession(RevokeSessionModel model, CancellationToken token = new())
    {
        var lastSession = await _dbContext.RefreshTokens.AsTracking().OrderByDescending(x => x.CreatedAt)
            .Where(x => x.SessionTokenId == null && x.AccountId == model.AccountId).FirstAsync(token);
        lastSession.RevokedAt = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(token);
    }

    public async Task Reconfirm(ReconfirmModel model, CancellationToken token = new())
    {
        var account = await _dbContext.Accounts.AsTracking().Where(x => x.Email == model.Email).SingleAsync(token);
        var confirmationToken = HashHelper.GenerateRandomToken();
        var confirmationExpiresAt = model.RequestedAt.Add(_accountOptions.LinkExpirationTime);

        account.ConfirmationToken = confirmationToken;
        account.ConfirmationExpiresAt = confirmationExpiresAt;
        account.ConfirmedAt = null;

        var confirmationEmail = new ReconfirmationEmail
        {
            ConfirmationToken = confirmationToken,
            ConfirmationExpiresAt = confirmationExpiresAt,
            Recipient = model.Email,
            Status = EmailStatus.Created
        };

        await _dbContext.Emails.AddAsync(confirmationEmail, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task ResetPassword(ResetPasswordModel model, CancellationToken token = new())
    {
        var account = await _dbContext.Accounts.AsTracking().Where(x => x.Email == model.Email).SingleAsync(token);
        var passwordResetToken = HashHelper.GenerateRandomToken();
        var passwordResetExpiresAt = model.RequestedAt.Add(_accountOptions.LinkExpirationTime);

        account.PasswordResetToken = passwordResetToken;
        account.PasswordResetExpiresAt = passwordResetExpiresAt;

        var passwordResetEmail = new PasswordResetEmail
        {
            PasswordResetToken = passwordResetToken,
            PasswordResetExpiresAt = passwordResetExpiresAt,
            Recipient = model.Email,
            Status = EmailStatus.Created
        };

        await _dbContext.Emails.AddAsync(passwordResetEmail, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task ConfirmPasswordReset(ConfirmPasswordResetModel model, CancellationToken token = new())
    {
        var account = await _dbContext.Accounts.AsTracking()
            .Where(x => x.PasswordResetToken == model.PasswordResetToken).SingleAsync(token);
        var password = _passwordProvider.GenerateRandomPassword();
        var passwordModel = HashHelper.HashPassword(password);

        account.PasswordHash = passwordModel.PasswordHash;
        account.PasswordSalt = passwordModel.PasswordSalt;
        account.PasswordResetToken = null;
        account.PasswordResetExpiresAt = null;

        var newPasswordEmail = new NewPasswordEmail
        {
            Recipient = account.Email,
            Status = EmailStatus.Created,
            Password = password,
            Type = EmailType.NewPassword
        };
        await _dbContext.Emails.AddAsync(newPasswordEmail, token);

        await _dbContext.SaveChangesAsync(token);
    }

    public async Task Confirm(ConfirmModel model, CancellationToken token = new())
    {
        var account = await _dbContext.Accounts.AsTracking()
            .Where(x => x.ConfirmationToken == model.ConfirmationToken).SingleAsync(token);

        account.ConfirmationToken = null;
        account.ConfirmationExpiresAt = null;
        account.ConfirmedAt = model.RequestedAt;

        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<AccessModel> Login(LoginModel model, CancellationToken token = new())
    {
        var dbAccount = await _dbContext.Accounts.Include(x => x.Roles).Where(x => x.Email == model.Email)
            .SingleAsync(token);
        return await LoginInner(dbAccount, token);
    }

    public async Task<AccessModel> Login(OAuthLoginModel model, CancellationToken token = new())
    {
        var dbAccount = await _dbContext.Accounts.Include(x => x.Roles).Include(x => x.ExternalAccounts)
            .Where(x => x.ExternalAccounts.Select(ea => ea.ExternalEmail).Contains(model.ExternalEmail))
            .SingleAsync(token);
        return await LoginInner(dbAccount, token);
    }

    public async Task<AccessModel> Refresh(RefreshModel model, CancellationToken token = new())
    {
        var currentRefreshToken = await _dbContext.RefreshTokens.AsTracking().Include(x => x.Account)
            .ThenInclude(x => x.Roles).Where(x => x.Value == model.RefreshToken)
            .SingleAsync(token);
        currentRefreshToken.UsedAt = DateTimeOffset.UtcNow;

        var refreshToken = new RefreshToken
        {
            AccountId = currentRefreshToken.AccountId, Value = HashHelper.GenerateRandomToken(),
            SessionTokenId = currentRefreshToken.SessionTokenId ?? currentRefreshToken.Id
        };
        await _dbContext.RefreshTokens.AddAsync(refreshToken, token);
        await _dbContext.SaveChangesAsync(token);

        var jwt = _jwtService.GetJwt(currentRefreshToken.Account);

        return new AccessModel
        {
            AccessToken = jwt.AccessToken, RefreshToken = refreshToken.Value,
            Roles = currentRefreshToken.Account.Roles.Select(x => x.Role).ToList()
        };
    }

    public async Task<List<AccountDto>> GetAll(ODataQueryOptions<AccountDto> query, CancellationToken token = new())
    {
        return await _dbContext.Accounts.GetQuery(_mapper, query).ToListAsync(token);
    }

    public async Task<AccountDto> GetAuthenticated(GetAuthenticatedModel model, CancellationToken token = new())
    {
        return await _dbContext.Accounts.Where(x => x.Id == model.AccountId)
            .ProjectTo<AccountDto>(_mapper.ConfigurationProvider, x => x.ExternalAccounts, x => x.Roles)
            .SingleAsync(token);
    }

    private async Task<AccessModel> LoginInner(Account account, CancellationToken token = new())
    {
        var jwt = _jwtService.GetJwt(account);
        var dbRefreshToken = new RefreshToken
        {
            AccountId = account.Id,
            Value = HashHelper.GenerateRandomToken()
        };
        await _dbContext.RefreshTokens.AddAsync(dbRefreshToken, token);
        await _dbContext.SaveChangesAsync(token);

        return new AccessModel
        {
            AccessToken = jwt.AccessToken, RefreshToken = dbRefreshToken.Value, ExpiresAt = jwt.ExpiresAt,
            Roles = account.Roles.Select(x => x.Role).ToList()
        };
    }
}