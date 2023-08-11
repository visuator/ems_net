using Ems.Core.Entities.Enums;

namespace Ems.Models.Dtos;

public class GroupDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public Course Course { get; set; }
    public string Name { get; set; }
    public List<StudentDto> Students { get; set; }
    public List<ClassDto> Classes { get; set; }
}