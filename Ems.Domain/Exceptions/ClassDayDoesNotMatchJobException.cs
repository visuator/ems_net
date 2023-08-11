using Ems.Domain.Constants;
using Ems.Infrastructure.Exceptions;

namespace Ems.Domain.Exceptions;

public class ClassDayDoesNotMatchJobException : JobException
{
    public ClassDayDoesNotMatchJobException() : base(ErrorMessages.ClassVersion.ClassDayDoesNotMatch)
    {
    }
}