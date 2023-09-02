using FluentValidation;

namespace Ems.Services.Validation;

public class ValidatorResolverService : IValidatorResolverService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidatorResolverService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IValidatorStateBuilder<T> ForModel<T>(T model)
    {
        return new ValidatorStateBuilder<T>(model, _serviceProvider.GetRequiredService<IValidator<T>>());
    }
}