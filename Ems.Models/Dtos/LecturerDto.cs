namespace Ems.Models.Dtos;

public class LecturerDto : EntityBaseDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public AccountDto Account { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public List<ClassDto> Classes { get; set; }
}