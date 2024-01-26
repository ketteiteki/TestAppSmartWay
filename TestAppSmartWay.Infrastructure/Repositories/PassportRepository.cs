using Dapper;
using Npgsql;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;

namespace TestAppSmartWay.Infrastructure.Repositories;

public class PassportRepository(string connectionString) : IPassportRepository
{
    public async Task<PassportEntity?> GetByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    select * from "PassportEntity"
                    where "Id" = @Id
                    """;

        return await connection.QueryFirstOrDefaultAsync<PassportEntity>(query, new { Id = id });
    }
    
    public async Task<PassportEntity> InsertAsync(PassportEntity passportEntity)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    insert into "PassportEntity" ("Type", "Number")
                    values (@Type, @Number)
                    returning *
                    """;
        
        return await connection.QueryFirstAsync<PassportEntity>(query, passportEntity);
    }
}