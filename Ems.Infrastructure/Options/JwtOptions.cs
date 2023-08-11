using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ems.Infrastructure.Options;

public class JwtOptions
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string SigningKey { get; set; }
    public string EncryptingKey { get; set; }
    public TimeSpan ExpirationTime { get; set; }
    public SecurityKey SigningSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
    public SecurityKey EncryptingSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EncryptingKey));
}