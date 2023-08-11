using System.Security.Cryptography;
using Ems.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Ems.Infrastructure.Services;

public static class HashHelper
{
    public static string GenerateRandomToken()
    {
        var tokenBytes = new byte[32];
        RandomNumberGenerator.Fill(tokenBytes);
        var token = Convert.ToBase64String(tokenBytes);
        return token;
    }

    public static PasswordModel HashPassword(string password)
    {
        var salt = new byte[8];
        RandomNumberGenerator.Fill(salt);
        var passwordStream = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 16, 16);
        var passwordHashBase64 = Convert.ToBase64String(passwordStream);
        var saltBase64 = Convert.ToBase64String(salt);

        return new PasswordModel { PasswordHash = passwordHashBase64, PasswordSalt = saltBase64 };
    }

    public static PasswordModel HashPassword(string password, string saltBase64)
    {
        var salt = Convert.FromBase64String(saltBase64);
        var passwordStream = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 16, 16);
        var passwordHashBase64 = Convert.ToBase64String(passwordStream);

        return new PasswordModel { PasswordHash = passwordHashBase64 };
    }
}