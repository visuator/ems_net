using Ems.Models;
using Ems.Models.Dtos;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface IAccountService
{
    Task<bool> Exists(string email, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
    Task<bool> IsConfirmed(string email, CancellationToken token = new());
    Task<bool> IsConfirmed(Guid id, CancellationToken token = new());
    Task<bool> AnyByConfirmationToken(string confirmationToken, CancellationToken token = new());
    Task<bool> AnyByPasswordResetToken(string passwordResetToken, CancellationToken token = new());

    Task<bool> CheckPasswordResetExpiration(string passwordResetToken, DateTime requestedAt,
        CancellationToken token = new());

    Task<bool> CheckPassword(string email, string password, DateTime requestedAt,
        CancellationToken token = new());

    Task<bool> CheckConfirmationExpiration(string confirmationToken, DateTime requestedAt,
        CancellationToken token = new());

    Task<bool> IsLocked(string email, DateTime requestedAt, CancellationToken token = new());
    Task<bool> IsRevoked(string refreshToken, CancellationToken token = new());
    Task<bool> ExternalAccountExists(Guid id, CancellationToken token = new());
    Task<bool> ExternalAccountExists(string externalEmail, CancellationToken token = new());

    Task RevokeSession(RevokeSessionModel accountId, CancellationToken token = new());
    Task Reconfirm(ReconfirmModel model, CancellationToken token = new());
    Task ResetPassword(ResetPasswordModel model, CancellationToken token = new());
    Task ConfirmPasswordReset(ConfirmPasswordResetModel model, CancellationToken token = new());
    Task Confirm(ConfirmModel model, CancellationToken token = new());
    Task<AccessModel> Login(LoginModel model, CancellationToken token = new());
    Task<AccessModel> Login(OAuthLoginModel model, CancellationToken token = new());
    Task<AccessModel> Refresh(RefreshModel model, CancellationToken token = new());
    Task<List<AccountDto>> GetAll(ODataQueryOptions<AccountDto> query, CancellationToken token = new());
    Task<AccountDto> GetAuthenticated(GetAuthenticatedModel model, CancellationToken token = new());
}