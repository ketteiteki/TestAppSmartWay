using Dapper;
using Npgsql;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;

namespace TestAppSmartWay.Infrastructure.Repositories;

public class EmployeeRepository(string connectionString) : IEmployeeRepository
{
    public async Task<EmployeeEntity?> GetByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    select * from "EmployeeEntity" e
                    left join "PassportEntity" p on p."Id" = e."PassportId"
                    left join "DepartmentEntity" d on d."Id" = e."DepartmentId"
                    where e."Id" = @Id
                    """;
        
        var data = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, EmployeeEntity>(query,
            (employeeEntity, passportEntity, departmentEntity) =>
            {
                employeeEntity.UpdatePassport(passportEntity);
                employeeEntity.UpdateDepartment(departmentEntity);
                return employeeEntity;
            }, new { Id = id });

        return data.FirstOrDefault();
    }
    
    public async Task<IEnumerable<EmployeeEntity>> GetEnumerableByCompanyIdAsync(int companyId, int offset, int limit)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var query = """
                    select * from "EmployeeEntity" e
                    left join "PassportEntity" p on p."Id" = e."PassportId"
                    left join "DepartmentEntity" d on d."Id" = e."DepartmentId"
                    where "CompanyId" = @CompanyId
                    offset @Offset limit @Limit
                    """;

        var data = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, EmployeeEntity>(query,
            (employeeEntity, passportEntity, departmentEntity) =>
            {
                employeeEntity.UpdatePassport(passportEntity);
                employeeEntity.UpdateDepartment(departmentEntity);
                return employeeEntity;
            }, new { CompanyId = companyId, Offset = offset, Limit = limit });

        return data;
    }

    public async Task<IEnumerable<EmployeeEntity>> GetEnumerableByDepartmentIdAsync(int departmentId, int offset, int limit)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    select * from "EmployeeEntity" e
                    left join "PassportEntity" p on p."Id" = e."PassportId"
                    left join "DepartmentEntity" d on d."Id" = e."DepartmentId"
                    where "DepartmentId" = @DepartmentId
                    offset @Offset limit @Limit
                    """;

        var data = await connection.QueryAsync<EmployeeEntity, PassportEntity, DepartmentEntity, EmployeeEntity>(query,
            (employeeEntity, passportEntity, departmentEntity) =>
            {
                employeeEntity.UpdatePassport(passportEntity);
                employeeEntity.UpdateDepartment(departmentEntity);
                return employeeEntity;
            }, new { DepartmentId = departmentId, Offset = offset, Limit = limit });

        return data;
    }

    public async Task<EmployeeEntity> InsertAsync(EmployeeEntity employeeEntity)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    insert into "EmployeeEntity" ("Name", "Surname", "Phone", "CompanyId", "PassportId", "DepartmentId") 
                    values (@Name, @Surname, @Phone, @CompanyId, @PassportId, @DepartmentId)
                    returning *
                    """;

        var param = new
        {
            employeeEntity.Id,
            employeeEntity.Name,
            employeeEntity.Surname,
            employeeEntity.Phone,
            employeeEntity.CompanyId,
            PassportId = employeeEntity.Passport.Id,
            DepartmentId = employeeEntity.Department.Id
        };
        
        return await connection.QueryFirstAsync<EmployeeEntity>(query, param);
    }

    public async Task<EmployeeEntity> UpdateAsync(EmployeeEntity employeeEntity)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        
        var query = """
                    update "EmployeeEntity" 
                    set "Name" = @Name, "Surname" = @Surname, "Phone" = @Phone, "CompanyId" = @CompanyId, 
                        "PassportId" = @PassportId, "DepartmentId" = @DepartmentId
                    where "Id" = @Id
                    returning *
                    """;
        
        var param = new
        {
            employeeEntity.Id,
            employeeEntity.Name,
            employeeEntity.Surname,
            employeeEntity.Phone,
            employeeEntity.CompanyId,
            PassportId = employeeEntity.Passport.Id,
            DepartmentId = employeeEntity.Department.Id
        };
        
        return await connection.QueryFirstAsync<EmployeeEntity>(query, param);
    }

    public async Task<EmployeeEntity> DeleteAsync(int id)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var query = """
                    delete from "EmployeeEntity"
                    where "Id" = @Id
                    returning *
                    """;

        return await connection.QueryFirstAsync<EmployeeEntity>(query, new { Id = id });
    }
}