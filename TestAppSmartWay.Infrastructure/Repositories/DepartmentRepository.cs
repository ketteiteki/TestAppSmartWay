using Dapper;
using Npgsql;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;

namespace TestAppSmartWay.Infrastructure.Repositories;

public class DepartmentRepository(string connectionString) : IDepartmentRepository
{
    public async Task<DepartmentEntity?> GetByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    select * from "DepartmentEntity"
                    where "Id" = @Id
                    """;

        return await connection.QueryFirstOrDefaultAsync<DepartmentEntity>(query, new { Id = id });
    }
    
    public async Task<DepartmentEntity> InsertAsync(DepartmentEntity departmentEntity)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    insert into "DepartmentEntity" ("Name", "Phone")
                    values (@Name, @Phone)
                    returning *
                    """;
        
        return await connection.QueryFirstAsync<DepartmentEntity>(query, departmentEntity);
    }
}