using Ems.Core.Entities.Enums;

namespace Ems.Models;

public class AddExternalAccountModel
{
    public ExternalAccountProvider ExternalAccountProvider { get; set; }
    public string ExternalEmail { get; set; }
    public Guid AccountId { get; set; }
}