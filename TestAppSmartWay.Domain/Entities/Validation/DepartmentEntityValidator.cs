using FluentValidation;
using TestAppSmartWay.Domain.Entities.Validation.PredicateValidators;

namespace TestAppSmartWay.Domain.Entities.Validation;

public class DepartmentEntityValidator : AbstractValidator<DepartmentEntity>
{
    public DepartmentEntityValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Phone).Must(CommonPredicates.ValidatePhone);
    }
}