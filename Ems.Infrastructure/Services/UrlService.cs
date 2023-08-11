using Ems.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Ems.Infrastructure.Services;

public class UrlService : IUrlService
{
    private readonly EmailOptions _emailOptions;

    public UrlService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public string GenerateConfirmationLink(string confirmationToken)
    {
        var url = new UriBuilder(_emailOptions.ConfirmationLink)
        {
            Query = $"confirmationToken={Uri.EscapeDataString(confirmationToken)}"
        };
        return url.ToString();
    }

    public string GeneratePasswordResetLink(string passwordResetToken)
    {
        var url = new UriBuilder(_emailOptions.PasswordResetLink)
        {
            Query = $"confirmationToken={Uri.EscapeDataString(passwordResetToken)}"
        };
        return url.ToString();
    }
}