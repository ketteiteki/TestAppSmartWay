using FluentValidation;
using TestAppSmartWay.Domain.Entities.Validation;
using TestAppSmartWay.Domain.Enum;

namespace TestAppSmartWay.Domain.Entities;

public class PassportEntity
{
    public int Id { get; private set; }
    
    public PassportType Type { get; private set; }
    
    public string Number { get; private set; }

    private static readonly PassportEntityValidator Validator = new();
    
    private PassportEntity() {}

    public PassportEntity(PassportType type, string number)
    {
        Type = type;
        Number = number;
        
        Validator.ValidateAndThrow(this);
    }

    public void UpdateId(int id)
    {
        Id = id;
        
        Validator.ValidateAndThrow(this);
    }
}