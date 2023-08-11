using Ems.Core.Entities.Enums;

namespace Ems.Models.Dtos;

public class ExternalAccountDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public AccountDto Account { get; set; }
    public string ExternalEmail { get; set; }
    public ExternalAccountProvider ExternalAccountProvider { get; set; }
}