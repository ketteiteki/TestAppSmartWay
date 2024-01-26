using FluentValidation;
using TestAppSmartWay.Domain.Entities.Validation;

namespace TestAppSmartWay.Domain.Entities;

public class EmployeeEntity
{
    public int Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string Surname { get; private set; }
    
    public string Phone { get; private set; }
    
    public int CompanyId { get; private set; }
    
    public PassportEntity Passport { get; private set; }

    public DepartmentEntity Department { get; private set; }

    private static readonly EmployeeEntityValidator Validator = new();
    
    private EmployeeEntity() {}

    public EmployeeEntity(string name, string surname, string phone, int companyId,
        PassportEntity passport, DepartmentEntity department)
    {
        Name = name;
        Surname = surname;
        Phone = phone;
        CompanyId = companyId;
        Passport = passport;
        Department = department;
        
        Validator.ValidateAndThrow(this);
    }

    public void UpdateName(string name)
    {
        Name = name;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdateSurname(string surname)
    {
        Surname = surname;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdatePhone(string phone)
    {
        Phone = phone;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdateCompanyId(int companyId)
    {
        CompanyId = companyId;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdatePassport(PassportEntity passport)
    {
        Passport = passport;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdateDepartment(DepartmentEntity department)
    {
        Department = department;
        
        Validator.ValidateAndThrow(this);
    }
}