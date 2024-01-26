using FluentValidation;
using TestAppSmartWay.Domain.Entities.Validation;

namespace TestAppSmartWay.Domain.Entities;

public class DepartmentEntity
{
    public int Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string Phone { get; private set; }

    private static readonly DepartmentEntityValidator Validator = new();
    
    private DepartmentEntity() {}

    public DepartmentEntity(string name, string phone)
    {
        Name = name;
        Phone = phone;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdateId(int id)
    {
        Id = id;
        
        Validator.ValidateAndThrow(this);
    }
}