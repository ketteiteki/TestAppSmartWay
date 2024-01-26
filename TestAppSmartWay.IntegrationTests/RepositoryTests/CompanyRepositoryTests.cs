#nullable disable

using Dapper;
using FluentAssertions;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.IntegrationTests.Helpers;
using Xunit;

namespace TestAppSmartWay.IntegrationTests.RepositoryTests;

public class CompanyRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task GetByIdTest()
    {
        var company = DomainHelper.CreateCompanyEntity();
        var insertCompanyResult = await CompanyRepository.InsertAsync(company);

        var foundCompany = await CompanyRepository.GetByIdAsync(insertCompanyResult.Id);

        foundCompany.Id.Should().Be(insertCompanyResult.Id);
        foundCompany.Name.Should().Be(insertCompanyResult.Name);
    }
    
    [Fact]
    public async Task InsertCompanyTest()
    {
        var company = DomainHelper.CreateCompanyEntity();

        var insertCompanyResult = await CompanyRepository.InsertAsync(company);
        
        var query = """
                    select * from "CompanyEntity"
                    where "Id" = @Id
                    """;
        var connection = GetConnection();
        var findCompanyResult = await connection.QueryFirstAsync<CompanyEntity>(query, insertCompanyResult);
        findCompanyResult.Name.Should().Be(company.Name);
    }
}