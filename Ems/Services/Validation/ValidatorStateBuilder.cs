using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ems.Services.Validation;

public class ValidatorStateBuilder<T> : IValidatorStateBuilder<T>
{
    private readonly T _model;
    private readonly IValidator<T> _validator;

    public ValidatorStateBuilder(T model, IValidator<T> validator)
    {
        _model = model;
        _validator = validator;
    }

    public IValidatorExecutorBuilder<T> WithModelState(ModelStateDictionary modelState)
    {
        return new ValidatorExecutorBuilder<T>(_model, _validator, modelState);
    }
}