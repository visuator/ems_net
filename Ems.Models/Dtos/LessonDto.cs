namespace Ems.Models.Dtos;

public class LessonDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<ClassDto> Classes { get; set; }
}