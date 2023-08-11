namespace Ems.Models;

public class JwtModel
{
    public string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}