using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ems.Core.Entities;
using Ems.Infrastructure.Options;
using Ems.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtConstants = Microsoft.IdentityModel.JsonWebTokens.JwtConstants;

namespace Ems.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public JwtModel GetJwt(Account account)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, account.Id.ToString())
        };
        claims.AddRange(account.Roles.Select(x => new Claim(ClaimTypes.Role, ((int)x.Role).ToString())));
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.Add(_jwtOptions.ExpirationTime).UtcDateTime;
        var issuedAt = now.UtcDateTime;

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.CreateJwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            new ClaimsIdentity(claims),
            null,
            expiresAt,
            issuedAt,
            new SigningCredentials(_jwtOptions.SigningSecurityKey, SecurityAlgorithms.HmacSha256),
            new EncryptingCredentials(_jwtOptions.EncryptingSecurityKey, JwtConstants.DirectKeyUseAlg,
                SecurityAlgorithms.Aes128CbcHmacSha256)
        );
        return new JwtModel { AccessToken = jwtHandler.WriteToken(jwtToken), ExpiresAt = expiresAt };
    }

    public JwtSecurityToken DecodeToken(string token)
    {
        return new JwtSecurityTokenHandler().ReadJwtToken(token);
    }
}