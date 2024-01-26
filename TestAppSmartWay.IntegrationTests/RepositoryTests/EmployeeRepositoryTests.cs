#nullable disable

using Dapper;
using FluentAssertions;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.IntegrationTests.Helpers;
using Xunit;

namespace TestAppSmartWay.IntegrationTests.RepositoryTests;

public class EmployeeRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task GetByIdTest()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var company = DomainHelper.CreateCompanyEntity();
        var department = DomainHelper.CreateDepartmentEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        var employee = DomainHelper.CreateEmployeeEntity(insertCompanyResult.Id, insertPassportResult, insertDepartmentResult);
        var insertEmployeeResult = await EmployeeRepository.InsertAsync(employee);

        var foundEmployee = await EmployeeRepository.GetByIdAsync(insertEmployeeResult.Id);

        foundEmployee.Id.Should().Be(insertEmployeeResult.Id);
        foundEmployee.Name.Should().Be(insertEmployeeResult.Name);
        foundEmployee.Surname.Should().Be(insertEmployeeResult.Surname);
        foundEmployee.Phone.Should().Be(insertEmployeeResult.Phone);
        foundEmployee.CompanyId.Should().Be(insertEmployeeResult.CompanyId);
        foundEmployee.Passport.Id.Should().Be(insertPassportResult.Id);
        foundEmployee.Department.Id.Should().Be(insertDepartmentResult.Id);
    }
    
    [Fact]
    public async Task InsertEmployeeTest()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var company = DomainHelper.CreateCompanyEntity();
        var department = DomainHelper.CreateDepartmentEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        var employee = DomainHelper.CreateEmployeeEntity(insertCompanyResult.Id, insertPassportResult, insertDepartmentResult);

        var insertEmployeeResult = await EmployeeRepository.InsertAsync(employee);

        var query = """
                    select * from "EmployeeEntity" e
                    left join "PassportEntity" p on p."Id" = e."PassportId"
                    left join "DepartmentEntity" d on d."Id" = e."DepartmentId"
                    where e."Id" = @Id 
                    limit 1
                    """;
        var connection = GetConnection();
        var findEmployeeEnumerableResult = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, EmployeeEntity>(query,
            (employeeEntity, passportEntity, departmentEntity) =>
            {
                employeeEntity.UpdatePassport(passportEntity);
                employeeEntity.UpdateDepartment(departmentEntity);
                return employeeEntity;
            }, insertEmployeeResult);
        var findEmployeeResult = findEmployeeEnumerableResult.First();
        findEmployeeResult.Name.Should().Be(employee.Name);
        findEmployeeResult.Surname.Should().Be(employee.Surname);
        findEmployeeResult.Phone.Should().Be(employee.Phone);
        findEmployeeResult.CompanyId.Should().Be(employee.CompanyId);
        findEmployeeResult.Passport.Type.Should().Be(passport.Type);
        findEmployeeResult.Passport.Number.Should().Be(passport.Number);
        findEmployeeResult.Department.Name.Should().Be(department.Name);
        findEmployeeResult.Department.Phone.Should().Be(department.Phone);
    }

    [Fact]
    public async Task GetEnumerableByCompanyIdTest()
    {
        var passportOfFirstEmployee = DomainHelper.CreatePassportEntity();
        var passportOfSecondEmployee = DomainHelper.CreatePassportEntity();
        var companyOfFirstAndSecondEmployee = DomainHelper.CreateCompanyEntity();
        var departmentOfFirstAndSecondEmployee = DomainHelper.CreateDepartmentEntity();
        var passportOfThirdEmployee = DomainHelper.CreatePassportEntity();
        var companyOfThirdEmployee = DomainHelper.CreateCompanyEntity();
        var departmentOfThirdEmployee = DomainHelper.CreateDepartmentEntity();
        var insertPassportOfFirstEmployeeResult = await PassportRepository.InsertAsync(passportOfFirstEmployee);
        var insertPassportOfSecondEmployeeResult = await PassportRepository.InsertAsync(passportOfSecondEmployee);
        var insertCompanyOfFirstAndSecondEmployeeResult = await CompanyRepository.InsertAsync(companyOfFirstAndSecondEmployee);
        var insertDepartmentOfFirstAndSecondEmployeeResult = await DepartmentRepository.InsertAsync(departmentOfFirstAndSecondEmployee);
        var insertPassportOfThirdEmployeeResult = await PassportRepository.InsertAsync(passportOfThirdEmployee);
        var insertCompanyOfThirdEmployeeResult = await CompanyRepository.InsertAsync(companyOfThirdEmployee);
        var insertDepartmentOfThirdEmployeeResult = await DepartmentRepository.InsertAsync(departmentOfThirdEmployee);
        var firstEmployee = DomainHelper.CreateEmployeeEntity(
            insertCompanyOfFirstAndSecondEmployeeResult.Id, 
            insertPassportOfFirstEmployeeResult, 
            insertDepartmentOfFirstAndSecondEmployeeResult);
        var secondEmployee = DomainHelper.CreateEmployeeEntity(
            insertCompanyOfFirstAndSecondEmployeeResult.Id, 
            insertPassportOfSecondEmployeeResult, 
            insertDepartmentOfFirstAndSecondEmployeeResult);
        var thirdEmployee = DomainHelper.CreateEmployeeEntity(
            insertCompanyOfThirdEmployeeResult.Id, 
            insertPassportOfThirdEmployeeResult, 
            insertDepartmentOfThirdEmployeeResult);
        var insertFirstEmployeeResult = await EmployeeRepository.InsertAsync(firstEmployee);
        var insertSecondEmployeeResult = await EmployeeRepository.InsertAsync(secondEmployee);
        var insertThirdEmployeeResult = await EmployeeRepository.InsertAsync(thirdEmployee);

        var getFirstAndSecondEmployeeByCompanyIdResult = 
            await EmployeeRepository.GetEnumerableByCompanyIdAsync(insertCompanyOfFirstAndSecondEmployeeResult.Id, 0, 10);
        var getThirdEmployeeByCompanyIdResult = 
            await EmployeeRepository.GetEnumerableByCompanyIdAsync(insertCompanyOfThirdEmployeeResult.Id, 0, 10);
        
        var firstAndSecondEmployeeByCompanyIdResultList = getFirstAndSecondEmployeeByCompanyIdResult.ToList();
        var foundFirstEmployee = firstAndSecondEmployeeByCompanyIdResultList.First(x => x.Id == insertFirstEmployeeResult.Id);
        var foundSecondEmployee = firstAndSecondEmployeeByCompanyIdResultList.First(x => x.Id == insertSecondEmployeeResult.Id);
        var foundThirdEmployee = getThirdEmployeeByCompanyIdResult.First(x => x.Id == insertThirdEmployeeResult.Id);
        foundFirstEmployee.Id.Should().Be(insertFirstEmployeeResult.Id);
        foundFirstEmployee.Name.Should().Be(insertFirstEmployeeResult.Name);
        foundFirstEmployee.Surname.Should().Be(insertFirstEmployeeResult.Surname);
        foundFirstEmployee.Phone.Should().Be(insertFirstEmployeeResult.Phone);
        foundFirstEmployee.CompanyId.Should().Be(insertFirstEmployeeResult.CompanyId);
        foundFirstEmployee.Passport.Id.Should().Be(insertPassportOfFirstEmployeeResult.Id);
        foundFirstEmployee.Passport.Number.Should().Be(insertPassportOfFirstEmployeeResult.Number);
        foundFirstEmployee.Passport.Type.Should().Be(insertPassportOfFirstEmployeeResult.Type);
        foundFirstEmployee.Department.Id.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Id);
        foundFirstEmployee.Department.Name.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Name);
        foundFirstEmployee.Department.Phone.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Phone);
        foundSecondEmployee.Id.Should().Be(insertSecondEmployeeResult.Id);
        foundSecondEmployee.Name.Should().Be(insertSecondEmployeeResult.Name);
        foundSecondEmployee.Surname.Should().Be(insertSecondEmployeeResult.Surname);
        foundSecondEmployee.Phone.Should().Be(insertSecondEmployeeResult.Phone);
        foundSecondEmployee.CompanyId.Should().Be(insertSecondEmployeeResult.CompanyId);
        foundSecondEmployee.Passport.Id.Should().Be(insertPassportOfSecondEmployeeResult.Id);
        foundSecondEmployee.Passport.Number.Should().Be(insertPassportOfSecondEmployeeResult.Number);
        foundSecondEmployee.Passport.Type.Should().Be(insertPassportOfSecondEmployeeResult.Type);
        foundSecondEmployee.Department.Id.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Id);
        foundSecondEmployee.Department.Name.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Name);
        foundSecondEmployee.Department.Phone.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Phone);
        foundThirdEmployee.Id.Should().Be(insertThirdEmployeeResult.Id);
        foundThirdEmployee.Name.Should().Be(insertThirdEmployeeResult.Name);
        foundThirdEmployee.Surname.Should().Be(insertThirdEmployeeResult.Surname);
        foundThirdEmployee.Phone.Should().Be(insertThirdEmployeeResult.Phone);
        foundThirdEmployee.CompanyId.Should().Be(insertThirdEmployeeResult.CompanyId);
        foundThirdEmployee.Passport.Id.Should().Be(insertPassportOfThirdEmployeeResult.Id);
        foundThirdEmployee.Passport.Number.Should().Be(insertPassportOfThirdEmployeeResult.Number);
        foundThirdEmployee.Passport.Type.Should().Be(insertPassportOfThirdEmployeeResult.Type);
        foundThirdEmployee.Department.Id.Should().Be(insertDepartmentOfThirdEmployeeResult.Id);
        foundThirdEmployee.Department.Name.Should().Be(insertDepartmentOfThirdEmployeeResult.Name);
        foundThirdEmployee.Department.Phone.Should().Be(insertDepartmentOfThirdEmployeeResult.Phone);
    }
    
    [Fact]
    public async Task GetEnumerableByDepartmentIdTest()
    {
        var passportOfFirstEmployee = DomainHelper.CreatePassportEntity();
        var passportOfSecondEmployee = DomainHelper.CreatePassportEntity();
        var companyOfFirstAndSecondEmployee = DomainHelper.CreateCompanyEntity();
        var departmentOfFirstAndSecondEmployee = DomainHelper.CreateDepartmentEntity();
        var passportOfThirdEmployee = DomainHelper.CreatePassportEntity();
        var companyOfThirdEmployee = DomainHelper.CreateCompanyEntity();
        var departmentOfThirdEmployee = DomainHelper.CreateDepartmentEntity();
        var insertPassportOfFirstEmployeeResult = await PassportRepository.InsertAsync(passportOfFirstEmployee);
        var insertPassportOfSecondEmployeeResult = await PassportRepository.InsertAsync(passportOfSecondEmployee);
        var insertCompanyOfFirstAndSecondEmployeeResult = await CompanyRepository.InsertAsync(companyOfFirstAndSecondEmployee);
        var insertDepartmentOfFirstAndSecondEmployeeResult = await DepartmentRepository.InsertAsync(departmentOfFirstAndSecondEmployee);
        var insertPassportOfThirdEmployeeResult = await PassportRepository.InsertAsync(passportOfThirdEmployee);
        var insertCompanyOfThirdEmployeeResult = await CompanyRepository.InsertAsync(companyOfThirdEmployee);
        var insertDepartmentOfThirdEmployeeResult = await DepartmentRepository.InsertAsync(departmentOfThirdEmployee);
        
        var firstEmployee = DomainHelper.CreateEmployeeEntity(insertCompanyOfFirstAndSecondEmployeeResult.Id, insertPassportOfFirstEmployeeResult, insertDepartmentOfFirstAndSecondEmployeeResult);
        var secondEmployee = DomainHelper.CreateEmployeeEntity(insertCompanyOfFirstAndSecondEmployeeResult.Id, insertPassportOfSecondEmployeeResult, insertDepartmentOfFirstAndSecondEmployeeResult);
        var thirdEmployee = DomainHelper.CreateEmployeeEntity(insertCompanyOfThirdEmployeeResult.Id, insertPassportOfThirdEmployeeResult, insertDepartmentOfThirdEmployeeResult);
        var insertFirstEmployeeResult = await EmployeeRepository.InsertAsync(firstEmployee);
        var insertSecondEmployeeResult = await EmployeeRepository.InsertAsync(secondEmployee);
        var insertThirdEmployeeResult = await EmployeeRepository.InsertAsync(thirdEmployee);

        var getFirstAndSecondEmployeeByCompanyIdResult = 
            await EmployeeRepository.GetEnumerableByDepartmentIdAsync(insertCompanyOfFirstAndSecondEmployeeResult.Id, 0, 10);
        var getThirdEmployeeByCompanyIdResult = 
            await EmployeeRepository.GetEnumerableByDepartmentIdAsync(insertCompanyOfThirdEmployeeResult.Id, 0, 10);
        var firstAndSecondEmployeeByCompanyIdResultList = getFirstAndSecondEmployeeByCompanyIdResult.ToList();
        var foundFirstEmployee = firstAndSecondEmployeeByCompanyIdResultList.First(x => x.Id == insertFirstEmployeeResult.Id);
        var foundSecondEmployee = firstAndSecondEmployeeByCompanyIdResultList.First(x => x.Id == insertSecondEmployeeResult.Id);
        var foundThirdEmployee = getThirdEmployeeByCompanyIdResult.First(x => x.Id == insertThirdEmployeeResult.Id);
        foundFirstEmployee.Id.Should().Be(insertFirstEmployeeResult.Id);
        foundFirstEmployee.Name.Should().Be(insertFirstEmployeeResult.Name);
        foundFirstEmployee.Surname.Should().Be(insertFirstEmployeeResult.Surname);
        foundFirstEmployee.Phone.Should().Be(insertFirstEmployeeResult.Phone);
        foundFirstEmployee.CompanyId.Should().Be(insertFirstEmployeeResult.CompanyId);
        foundFirstEmployee.Passport.Id.Should().Be(insertPassportOfFirstEmployeeResult.Id);
        foundFirstEmployee.Passport.Number.Should().Be(insertPassportOfFirstEmployeeResult.Number);
        foundFirstEmployee.Passport.Type.Should().Be(insertPassportOfFirstEmployeeResult.Type);
        foundFirstEmployee.Department.Id.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Id);
        foundFirstEmployee.Department.Name.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Name);
        foundFirstEmployee.Department.Phone.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Phone);
        foundSecondEmployee.Id.Should().Be(insertSecondEmployeeResult.Id);
        foundSecondEmployee.Name.Should().Be(insertSecondEmployeeResult.Name);
        foundSecondEmployee.Surname.Should().Be(insertSecondEmployeeResult.Surname);
        foundSecondEmployee.Phone.Should().Be(insertSecondEmployeeResult.Phone);
        foundSecondEmployee.CompanyId.Should().Be(insertSecondEmployeeResult.CompanyId);
        foundSecondEmployee.Passport.Id.Should().Be(insertPassportOfSecondEmployeeResult.Id);
        foundSecondEmployee.Passport.Number.Should().Be(insertPassportOfSecondEmployeeResult.Number);
        foundSecondEmployee.Passport.Type.Should().Be(insertPassportOfSecondEmployeeResult.Type);
        foundSecondEmployee.Department.Id.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Id);
        foundSecondEmployee.Department.Name.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Name);
        foundSecondEmployee.Department.Phone.Should().Be(insertDepartmentOfFirstAndSecondEmployeeResult.Phone);
        foundThirdEmployee.Id.Should().Be(insertThirdEmployeeResult.Id);
        foundThirdEmployee.Name.Should().Be(insertThirdEmployeeResult.Name);
        foundThirdEmployee.Surname.Should().Be(insertThirdEmployeeResult.Surname);
        foundThirdEmployee.Phone.Should().Be(insertThirdEmployeeResult.Phone);
        foundThirdEmployee.CompanyId.Should().Be(insertThirdEmployeeResult.CompanyId);
        foundThirdEmployee.Passport.Id.Should().Be(insertPassportOfThirdEmployeeResult.Id);
        foundThirdEmployee.Passport.Number.Should().Be(insertPassportOfThirdEmployeeResult.Number);
        foundThirdEmployee.Passport.Type.Should().Be(insertPassportOfThirdEmployeeResult.Type);
        foundThirdEmployee.Department.Id.Should().Be(insertDepartmentOfThirdEmployeeResult.Id);
        foundThirdEmployee.Department.Name.Should().Be(insertDepartmentOfThirdEmployeeResult.Name);
        foundThirdEmployee.Department.Phone.Should().Be(insertDepartmentOfThirdEmployeeResult.Phone);
    }
    
    [Fact]
    public async Task UpdateEmployeeTest()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var company = DomainHelper.CreateCompanyEntity();
        var department = DomainHelper.CreateDepartmentEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        var employee = DomainHelper.CreateEmployeeEntity(insertCompanyResult.Id, insertPassportResult, insertDepartmentResult);
        var insertEmployeeResult = await EmployeeRepository.InsertAsync(employee);
        var newName = Guid.NewGuid().ToString();
        var newSurname = Guid.NewGuid().ToString();
        var newPhone = "+71111111111";
        var newPassport = DomainHelper.CreatePassportEntity();
        var newCompany = DomainHelper.CreateCompanyEntity();
        var newDepartment = DomainHelper.CreateDepartmentEntity();
        var insertNewPassportResult = await PassportRepository.InsertAsync(newPassport);
        var insertNewCompanyResult = await CompanyRepository.InsertAsync(newCompany);
        var insertNewDepartmentResult = await DepartmentRepository.InsertAsync(newDepartment);
        
        insertEmployeeResult.UpdateName(newName);
        insertEmployeeResult.UpdateSurname(newSurname);
        insertEmployeeResult.UpdatePhone(newPhone);
        insertEmployeeResult.UpdateCompanyId(insertNewCompanyResult.Id);
        insertEmployeeResult.UpdatePassport(insertNewPassportResult);
        insertEmployeeResult.UpdateDepartment(insertNewDepartmentResult);
        await EmployeeRepository.UpdateAsync(insertEmployeeResult);
        
        var query = """
                    select * from "EmployeeEntity" e
                    left join "PassportEntity" p on p."Id" = e."PassportId"
                    left join "DepartmentEntity" d on d."Id" = e."DepartmentId"
                    where e."Id" = @Id 
                    limit 1
                    """;
        var connection = GetConnection();
        var findEmployeeEnumerableResult = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, EmployeeEntity>(query,
            (employeeEntity, passportEntity, departmentEntity) =>
            {
                employeeEntity.UpdatePassport(passportEntity);
                employeeEntity.UpdateDepartment(departmentEntity);
                return employeeEntity;
            }, insertEmployeeResult);
        var findEmployeeResult = findEmployeeEnumerableResult.First();
        findEmployeeResult.Name.Should().Be(newName);
        findEmployeeResult.Surname.Should().Be(newSurname);
        findEmployeeResult.Phone.Should().Be(newPhone);
        findEmployeeResult.CompanyId.Should().Be(insertNewCompanyResult.Id);
        findEmployeeResult.Passport.Id.Should().Be(insertNewPassportResult.Id);
        findEmployeeResult.Department.Id.Should().Be(insertNewDepartmentResult.Id);
    }
    
    [Fact]
    public async Task DeleteEmployeeTest()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var company = DomainHelper.CreateCompanyEntity();
        var department = DomainHelper.CreateDepartmentEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        var employee = DomainHelper.CreateEmployeeEntity(insertCompanyResult.Id, insertPassportResult, insertDepartmentResult);
        var insertEmployeeResult = await EmployeeRepository.InsertAsync(employee);
        
        await EmployeeRepository.DeleteAsync(insertEmployeeResult.Id);
        
        var query = """
                    select * from "EmployeeEntity" e
                    left join "PassportEntity" p on p."Id" = e."PassportId"
                    left join "DepartmentEntity" d on d."Id" = e."DepartmentId"
                    where e."Id" = @Id 
                    limit 1
                    """;
        var connection = GetConnection();
        var findEmployeeEnumerableResult = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, EmployeeEntity>(query,
            (employeeEntity, passportEntity, departmentEntity) =>
            {
                employeeEntity.UpdatePassport(passportEntity);
                employeeEntity.UpdateDepartment(departmentEntity);
                return employeeEntity;
            }, insertEmployeeResult);
        var findEmployeeResult = findEmployeeEnumerableResult.FirstOrDefault();
        findEmployeeResult.Should().BeNull();
    }
}