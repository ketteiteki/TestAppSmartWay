using Dapper;
using Npgsql;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;

namespace TestAppSmartWay.Infrastructure.Repositories;

public class CompanyRepository(string connectionString) : ICompanyRepository
{
    public async Task<CompanyEntity?> GetByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    select * from "CompanyEntity"
                    where "Id" = @Id
                    """;

        return await connection.QueryFirstOrDefaultAsync<CompanyEntity>(query, new { Id = id });
    }

    public async Task<CompanyEntity> InsertAsync(CompanyEntity companyEntity)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    insert into "CompanyEntity" ("Name")
                    values (@Name)
                    returning *
                    """;
        
        return await connection.QueryFirstAsync<CompanyEntity>(query, companyEntity);
    }
}