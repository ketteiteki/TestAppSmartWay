using FluentValidation;
using TestAppSmartWay.Domain.Entities.Validation;

namespace TestAppSmartWay.Domain.Entities;

public class CompanyEntity
{
    public int Id { get; private set; }
    
    public string Name { get; private set; }

    private static readonly CompanyEntityValidator Validator = new();
    
    private CompanyEntity() {}

    public CompanyEntity(string name)
    {
        Name = name;

        Validator.ValidateAndThrow(this);
    }
}