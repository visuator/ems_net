using System.ComponentModel.DataAnnotations;

namespace Ems.Models;

public class LoginModel
{
    [EmailAddress] public string Email { get; set; }

    public string Password { get; set; }
    public DateTime RequestedAt { get; set; }
}