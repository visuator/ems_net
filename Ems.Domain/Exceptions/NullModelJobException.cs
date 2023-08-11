using Ems.Domain.Constants;
using Ems.Infrastructure.Exceptions;

namespace Ems.Domain.Exceptions;

public class NullModelJobException : JobException
{
    public NullModelJobException() : base(ErrorMessages.Job.NullModel)
    {
    }
}