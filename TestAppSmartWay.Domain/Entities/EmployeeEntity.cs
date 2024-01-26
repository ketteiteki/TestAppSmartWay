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
    
    private EmployeeEntity() {}

    public static EmployeeEntity Create(string name, string surname, string phone, int companyId, 
        PassportEntity passport, DepartmentEntity department)
    {
        return new EmployeeEntity
        {
            Name = name,
            Surname = surname,
            Phone = phone,
            CompanyId = companyId,
            Passport = passport,
            Department = department
        };
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
    
    public void UpdateSurname(string surname)
    {
        Surname = surname;
    }
    
    public void UpdatePhone(string phone)
    {
        Phone = phone;
    }
    
    public void UpdateCompanyId(int companyId)
    {
        CompanyId = companyId;
    }
    
    public void UpdatePassport(PassportEntity passport)
    {
        Passport = passport;
    }
    
    public void UpdateDepartment(DepartmentEntity department)
    {
        Department = department;
    }
}