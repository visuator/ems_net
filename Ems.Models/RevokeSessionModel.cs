using Microsoft.AspNetCore.Mvc;

namespace Ems.Models;

public class RevokeSessionModel
{
    [FromRoute] public Guid AccountId { get; set; }
}