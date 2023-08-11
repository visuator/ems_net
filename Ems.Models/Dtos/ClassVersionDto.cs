using Ems.Core.Entities.Enums;

namespace Ems.Models.Dtos;

public class ClassVersionDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public ClassVersionStatus Status { get; set; }
    public string Name { get; set; }
    public List<ClassDto> Classes { get; set; }
}