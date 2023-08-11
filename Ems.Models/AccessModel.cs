using Ems.Core.Entities.Enums;

namespace Ems.Models;

public class AccessModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public List<Role> Roles { get; set; }
}