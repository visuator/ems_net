using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentValidation;

namespace Ems.Models;

public class GoogleOAuthModel : IAuthenticated
{
    [Required] public string Credential { get; set; }

    [JsonIgnore] public Guid AccountId { get; set; }

    public class Validator : AbstractValidator<GoogleOAuthModel>
    {
    }
}