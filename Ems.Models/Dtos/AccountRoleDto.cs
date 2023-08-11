using Ems.Core.Entities.Enums;

namespace Ems.Models.Dtos;

public class AccountRoleDto
{
    public Guid AccountId { get; set; }
    public AccountDto Account { get; set; }
    public Role Role { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}