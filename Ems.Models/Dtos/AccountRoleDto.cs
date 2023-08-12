using Ems.Core.Entities.Enums;

namespace Ems.Models.Dtos;

public class AccountRoleDto : EntityBaseDto
{
    public Guid AccountId { get; set; }
    public AccountDto Account { get; set; }
    public Role Role { get; set; }
}