using Ems.Infrastructure.Options;
using Microsoft.Extensions.Options;
using PasswordGenerator;

namespace Ems.Infrastructure.Services;

public class PasswordProvider : IPasswordProvider
{
    private readonly AccountOptions _accountOptions;

    public PasswordProvider(IOptions<AccountOptions> accountOptions)
    {
        _accountOptions = accountOptions.Value;
    }

    public string GenerateRandomPassword()
    {
        var password = new Password(true, true, true, true, _accountOptions.PasswordLength);
        return password.Next();
    }
}