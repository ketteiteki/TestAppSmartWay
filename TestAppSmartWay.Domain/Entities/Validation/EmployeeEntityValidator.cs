using FluentValidation;
using TestAppSmartWay.Domain.Entities.Validation.PredicateValidators;

namespace TestAppSmartWay.Domain.Entities.Validation;

public class EmployeeEntityValidator : AbstractValidator<EmployeeEntity>
{
    public EmployeeEntityValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
        RuleFor(x => x.Phone).Must(CommonPredicates.ValidatePhone);
    }
}