namespace Ems.Infrastructure.Services;

public interface IUrlService
{
    string GenerateConfirmationLink(string confirmationToken);
    string GeneratePasswordResetLink(string passwordResetToken);
}