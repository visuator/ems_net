using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ems.Models;

public class GoogleOAuthModel : IAuthenticated
{
    [Required] public string Credential { get; set; }

    [JsonIgnore] public Guid AccountId { get; set; }
}