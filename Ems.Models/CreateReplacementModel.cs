using System.Text.Json.Serialization;

namespace Ems.Models;

public class CreateReplacementModel
{
    [JsonIgnore]
    public Guid SourceClassId { get; set; }
    public Guid LecturerId { get; set; }
    public Guid LessonId { get; set; }
    public Guid ClassroomId { get; set; }
}