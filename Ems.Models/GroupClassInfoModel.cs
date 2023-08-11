using Ems.Domain.Enums;
using Ems.Models.Dtos;

namespace Ems.Models;

public class GroupClassInfoModel : ClassDto
{
    public GroupClassStatus Status { get; set; }
}