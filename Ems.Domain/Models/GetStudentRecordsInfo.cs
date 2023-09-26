using Ems.Models;

namespace Ems.Domain.Models;

public class GetStudentRecordsInfo : IRequestTimeStamp
{
    public Guid GroupId { get; set; }
    public DateTime RequestedAt { get; set; }
}