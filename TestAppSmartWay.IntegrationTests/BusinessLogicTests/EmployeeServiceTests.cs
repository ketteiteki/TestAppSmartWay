using System.Text.Json;
using Dapper;
using FluentAssertions;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Domain.Responses.Errors;
using TestAppSmartWay.IntegrationTests.Helpers;
using Xunit;

namespace TestAppSmartWay.IntegrationTests.BusinessLogicTests;

public class EmployeeServiceTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateEmployeeTest_Success()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var company = DomainHelper.CreateCompanyEntity();
        var department = DomainHelper.CreateDepartmentEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        var name = Guid.NewGuid().ToString();
        var surname = Guid.NewGuid().ToString();
        var phone = "+70000000000";
        
        var insertEmployeeResult = await EmployeeService.CreateEmployeeAsync(
            name,
            surname,
            phone,
            insertCompanyResult.Id,
            insertPassportResult.Id,
            insertDepartmentResult.Id);

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
            }, new { Id = insertEmployeeResult.Response });
        var findEmployeeResult = findEmployeeEnumerableResult.First();
        findEmployeeResult.Name.Should().Be(name);
        findEmployeeResult.Surname.Should().Be(surname);
        findEmployeeResult.Phone.Should().Be(phone);
        findEmployeeResult.CompanyId.Should().Be(insertCompanyResult.Id);
        findEmployeeResult.Passport.Type.Should().Be(passport.Type);
        findEmployeeResult.Passport.Number.Should().Be(passport.Number);
        findEmployeeResult.Department.Name.Should().Be(department.Name);
        findEmployeeResult.Department.Phone.Should().Be(department.Phone);
    }
    
    [Fact]
    public async Task CreateEmployeeTest_ThrowPassportNotFound()
    {
        var company = DomainHelper.CreateCompanyEntity();
        var department = DomainHelper.CreateDepartmentEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        var name = Guid.NewGuid().ToString();
        var surname = Guid.NewGuid().ToString();
        var phone = "+70000000000";
        
        var insertEmployeeResult = await EmployeeService.CreateEmployeeAsync(
            name,
            surname,
            phone,
            insertCompanyResult.Id,
            1,
            insertDepartmentResult.Id);

        insertEmployeeResult.Error.Should().BeOfType<Error>();
    }
    
    [Fact]
    public async Task CreateEmployeeTest_ThrowCompanyNotFound()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var department = DomainHelper.CreateDepartmentEntity();
        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        var name = Guid.NewGuid().ToString();
        var surname = Guid.NewGuid().ToString();
        var phone = "+70000000000";
        
        var insertEmployeeResult = await EmployeeService.CreateEmployeeAsync(
            name,
            surname,
            phone,
            1,
            insertPassportResult.Id,
            insertDepartmentResult.Id);
        
        insertEmployeeResult.Error.Should().BeOfType<Error>();
    }
    
    [Fact]
    public async Task CreateEmployeeTest_ThrowDepartmentNotFound()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var company = DomainHelper.CreateCompanyEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        var name = Guid.NewGuid().ToString();
        var surname = Guid.NewGuid().ToString();
        var phone = "+70000000000";
        
        var insertEmployeeResult = await EmployeeService.CreateEmployeeAsync(
            name,
            surname,
            phone,
            insertCompanyResult.Id,
            insertPassportResult.Id,
            1);
        
        insertEmployeeResult.Error.Should().BeOfType<Error>();
    }
    
    [Fact]
    public async Task UpdateEmployeeTest_Success()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var company = DomainHelper.CreateCompanyEntity();
        var department = DomainHelper.CreateDepartmentEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        var employee = DomainHelper.CreateEmployeeEntity(insertCompanyResult.Id, insertPassportResult, insertDepartmentResult);
        var nameForUpdate = "nameForUpdate";
        var surnameForUpdate = "surnameForUpdate";
        var phoneForUpdate = "surnameForUpdate";
        var passportForUpdate = DomainHelper.CreatePassportEntity();
        var companyForUpdate = DomainHelper.CreateCompanyEntity();
        var departmentForUpdate = DomainHelper.CreateDepartmentEntity();
        var insertCompanyForUpdateResult = await CompanyRepository.InsertAsync(companyForUpdate);
        var insertPassportForUpdateResult = await PassportRepository.InsertAsync(passportForUpdate);
        var insertDepartmentForUpdateResult = await DepartmentRepository.InsertAsync(departmentForUpdate);
        var insertEmployeeResult = await EmployeeRepository.InsertAsync(employee);

        var dictionary = new Dictionary<string, JsonElement>
        {
            { nameof(insertEmployeeResult.Name), JsonSerializer.SerializeToElement(nameForUpdate) },
            { nameof(insertEmployeeResult.Surname), JsonSerializer.SerializeToElement(surnameForUpdate) },
            { nameof(insertEmployeeResult.Phone), JsonSerializer.SerializeToElement(phoneForUpdate) },
            { nameof(insertEmployeeResult.CompanyId), JsonSerializer.SerializeToElement(insertCompanyForUpdateResult.Id) },
            { "PassportId", JsonSerializer.SerializeToElement(insertPassportForUpdateResult.Id) },
            { "DepartmentId", JsonSerializer.SerializeToElement(insertDepartmentForUpdateResult.Id) },
        };
        await EmployeeService.UpdateEmployeeAsync(insertEmployeeResult.Id, dictionary);
        
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
        findEmployeeResult.Name.Should().Be(nameForUpdate);
        findEmployeeResult.Surname.Should().Be(surnameForUpdate);
        findEmployeeResult.Phone.Should().Be(phoneForUpdate);
        findEmployeeResult.CompanyId.Should().Be(insertCompanyForUpdateResult.Id);
        findEmployeeResult.Passport.Id.Should().Be(insertPassportForUpdateResult.Id);
        findEmployeeResult.Passport.Type.Should().Be(insertPassportForUpdateResult.Type);
        findEmployeeResult.Passport.Number.Should().Be(insertPassportForUpdateResult.Number);
        findEmployeeResult.Department.Id.Should().Be(insertDepartmentForUpdateResult.Id);
        findEmployeeResult.Department.Name.Should().Be(insertDepartmentForUpdateResult.Name);
        findEmployeeResult.Department.Phone.Should().Be(insertDepartmentForUpdateResult.Phone);
    }
    
    [Fact]
    public async Task UpdateEmployeeTest_ThrowEmployeeNotFound()
    {
        var insertEmployeeResult = await EmployeeService.UpdateEmployeeAsync(1, new Dictionary<string, JsonElement>());

        insertEmployeeResult.Error.Should().BeOfType<Error>();
    }
    
    [Fact]
    public async Task DeleteEmployeeTest_Success()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var company = DomainHelper.CreateCompanyEntity();
        var department = DomainHelper.CreateDepartmentEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        var employee = DomainHelper.CreateEmployeeEntity(insertCompanyResult.Id, insertPassportResult, insertDepartmentResult);
        var insertEmployeeResult = await EmployeeRepository.InsertAsync(employee);

        await EmployeeService.DeleteEmployeeAsync(insertEmployeeResult.Id);
        
        var query = """
                    select * from "EmployeeEntity"
                    where "Id" = @Id
                    """;
        var connection = GetConnection();
        var foundEmployeeResult = await connection.QueryFirstOrDefaultAsync<EmployeeEntity>(query, new { insertEmployeeResult.Id });
        foundEmployeeResult.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteEmployeeTest_ThrowEmployeeNotFound()
    {
        var result = await EmployeeService.DeleteEmployeeAsync(1);

        result.Error.Should().BeOfType<Error>();
    }
}