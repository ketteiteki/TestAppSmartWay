using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Domain.Enum;

namespace TestAppSmartWay.IntegrationTests.Helpers;

public class DomainHelper
{
    public static EmployeeEntity CreateEmployeeEntity(int companyId, PassportEntity passport, DepartmentEntity department)
    {
        var employee = new EmployeeEntity(
            Guid.NewGuid().ToString(), 
            Guid.NewGuid().ToString(), 
            "+70000000000", 
            companyId, 
            passport, 
            department);
        
        return employee;
    }
    
    public static PassportEntity CreatePassportEntity()
    {
        var random = new Random();
        var number1 = random.Next(1000, 9999);
        var number2 = random.Next(100000, 999999);
        
        return new PassportEntity(PassportType.Regular, $"{number1} {number2}");
    }
    
    public static DepartmentEntity CreateDepartmentEntity()
    {
        var random = new Random();
        var number1 = random.Next(1, 999);
        var number2 = random.Next(100, 999);
        var number3 = random.Next(10, 99);
        var number4 = random.Next(10, 99);

        var phone = string.Concat("+", number1, number2, number3, number4);
        
        return new DepartmentEntity(Guid.NewGuid().ToString(), phone);
    }

    public static CompanyEntity CreateCompanyEntity()
    {
        return new CompanyEntity(Guid.NewGuid().ToString());
    }
}