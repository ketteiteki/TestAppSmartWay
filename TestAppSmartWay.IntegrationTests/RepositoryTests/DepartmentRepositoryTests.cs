#nullable disable

using Dapper;
using FluentAssertions;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.IntegrationTests.Helpers;
using Xunit;

namespace TestAppSmartWay.IntegrationTests.RepositoryTests;

public class DepartmentRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task GetByIdTest()
    {
        var department = DomainHelper.CreateDepartmentEntity();
        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);

        var foundDepartment = await DepartmentRepository.GetByIdAsync(insertDepartmentResult.Id);

        foundDepartment.Id.Should().Be(insertDepartmentResult.Id);
        foundDepartment.Name.Should().Be(insertDepartmentResult.Name);
        foundDepartment.Phone.Should().Be(insertDepartmentResult.Phone);
    }
    
    [Fact]
    public async Task InsertDepartmentTest()
    {
        var department = DomainHelper.CreateDepartmentEntity();

        var insertDepartmentResult = await DepartmentRepository.InsertAsync(department);
        
        var query = """
                    select * from "DepartmentEntity"
                    where "Id" = @Id
                    """;
        var connection = GetConnection();
        var findDepartmentResult = await connection.QueryFirstAsync<DepartmentEntity>(query, insertDepartmentResult);
        findDepartmentResult.Name.Should().Be(department.Name);
        findDepartmentResult.Phone.Should().Be(department.Phone);
    }
}