using Ems.Core.Entities.Enums;

namespace Ems.Models.Dtos;

public class SettingDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public Quarter CurrentQuarter { get; set; }
}