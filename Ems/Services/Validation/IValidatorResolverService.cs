namespace Ems.Services.Validation;

public interface IValidatorResolverService
{
    IValidatorStateBuilder<T> ForModel<T>(T model);
}