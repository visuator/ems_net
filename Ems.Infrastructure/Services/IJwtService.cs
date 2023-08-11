using System.IdentityModel.Tokens.Jwt;
using Ems.Core.Entities;
using Ems.Models;

namespace Ems.Infrastructure.Services;

public interface IJwtService
{
    JwtModel GetJwt(Account account);
    JwtSecurityToken DecodeToken(string token);
}