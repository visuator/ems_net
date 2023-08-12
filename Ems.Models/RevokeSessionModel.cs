using Microsoft.AspNetCore.Mvc;

namespace Ems.Models;

public class RevokeSessionModel
{
    public Guid AccountId { get; set; }
    public DateTime RequestedAt { get; set; }
}