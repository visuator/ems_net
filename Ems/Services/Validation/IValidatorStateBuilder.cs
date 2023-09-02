using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ems.Services.Validation;

public interface IValidatorStateBuilder<T>
{
    IValidatorExecutorBuilder<T> WithModelState(ModelStateDictionary modelState);
}