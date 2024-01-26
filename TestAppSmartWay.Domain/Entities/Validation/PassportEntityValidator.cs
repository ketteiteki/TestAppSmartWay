using FluentValidation;

namespace TestAppSmartWay.Domain.Entities.Validation;

public class PassportEntityValidator : AbstractValidator<PassportEntity>
{
    public PassportEntityValidator()
    {
        RuleFor(x => x.Number);
    }
}