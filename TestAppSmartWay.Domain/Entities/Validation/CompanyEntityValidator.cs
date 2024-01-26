using FluentValidation;
using TestAppSmartWay.Domain.Entities.Validation.PredicateValidators;

namespace TestAppSmartWay.Domain.Entities.Validation;

public class CompanyEntityValidator : AbstractValidator<DepartmentEntity>
{
    public CompanyEntityValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Phone).Must(CommonPredicates.ValidatePhone);
    }
}