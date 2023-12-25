using Ems.Domain.Services;
using FluentValidation;
using System.Text.Json.Serialization;

namespace Ems.Domain.Models;

public class CreateReplacementModel
{
    [JsonIgnore] public Guid SourceClassId { get; set; }
    public Guid LecturerId { get; set; }
    public Guid LessonId { get; set; }
    public Guid ClassroomId { get; set; }

    public class Validator : AbstractValidator<CreateReplacementModel>
    {
        public Validator(IClassService classService, ILecturerService lecturerService,
            IClassroomService classroomService, ILessonService lessonService)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .MustAsync(async (model, token) => await classService.Exists(model.SourceClassId, token))
                //.WithMessage(ErrorMessages.Class.IsNotExists)
                .MustAsync(async (model, token) => await lecturerService.Exists(model.LecturerId, token))
                //.WithMessage(ErrorMessages.Lecturer.IsNotExists)
                .MustAsync(async (model, token) => await lessonService.Exists(model.LessonId, token))
                //.WithMessage(ErrorMessages.Lecturer.IsNotExists)
                .MustAsync(async (model, token) => await classroomService.Exists(model.ClassroomId, token))
                //.WithMessage(ErrorMessages.Classroom.IsNotExists)
                .WithName(nameof(CreateReplacementModel));
        }
    }
}