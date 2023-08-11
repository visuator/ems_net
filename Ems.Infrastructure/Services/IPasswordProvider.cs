namespace Ems.Infrastructure.Services;

public interface IPasswordProvider
{
    string GenerateRandomPassword();
}