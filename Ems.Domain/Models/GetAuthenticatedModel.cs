using System.Text.Json.Serialization;
using Ems.Models;

namespace Ems.Domain.Models;

public class GetAuthenticatedModel : IAuthenticated
{
    [JsonIgnore] public Guid AccountId { get; set; }
}