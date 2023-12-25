namespace Ems.Models.Dtos;

public class StudentDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public GroupDto Group { get; set; }
    public Guid AccountId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
}