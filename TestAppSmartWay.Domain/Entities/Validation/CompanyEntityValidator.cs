using FluentValidation;
using TestAppSmartWay.Domain.Entities.Validation.PredicateValidators;

namespace TestAppSmartWay.Domain.Entities.Validation;

public class CompanyEntityValidator : AbstractValidator<CompanyEntity>
{
    public CompanyEntityValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}