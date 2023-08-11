using System.Text.Json.Serialization;

namespace Ems.Models;

public class GetAuthenticatedModel : IAuthenticated
{
    [JsonIgnore] public Guid AccountId { get; set; }
}