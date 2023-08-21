using Ems.Models.Dtos;
using Ems.Models.Enums;

namespace Ems.Models;

public class GroupClassInfoModel : ClassDto
{
    public GroupClassStatus Status { get; set; }
}