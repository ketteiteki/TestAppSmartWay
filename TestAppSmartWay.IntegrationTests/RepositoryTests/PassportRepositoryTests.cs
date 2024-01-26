#nullable disable

using Dapper;
using FluentAssertions;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.IntegrationTests.Helpers;
using Xunit;

namespace TestAppSmartWay.IntegrationTests.RepositoryTests;

public class PassportRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task GetByIdTest()
    {
        var passport = DomainHelper.CreatePassportEntity();
        var insertPassportResult = await PassportRepository.InsertAsync(passport);

        var foundPassport = await PassportRepository.GetByIdAsync(insertPassportResult.Id);

        foundPassport.Id.Should().Be(insertPassportResult.Id);
        foundPassport.Type.Should().Be(insertPassportResult.Type);
        foundPassport.Number.Should().Be(insertPassportResult.Number);
    }
    
    [Fact]
    public async Task InsertPassportTest()
    {
        var passport = DomainHelper.CreatePassportEntity();

        var insertPassportResult = await PassportRepository.InsertAsync(passport);
        
        var query = """
                    select * from "PassportEntity"
                    where "Id" = @Id
                    """;
        var connection = GetConnection();
        var findPassportResult = await connection.QueryFirstAsync<PassportEntity>(query, insertPassportResult);
        findPassportResult.Type.Should().Be(passport.Type);
        findPassportResult.Number.Should().Be(passport.Number);
    }
}