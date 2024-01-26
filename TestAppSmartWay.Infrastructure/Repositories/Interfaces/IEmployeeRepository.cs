using TestAppSmartWay.Domain.Entities;

namespace TestAppSmartWay.Infrastructure.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<EmployeeEntity?> GetByIdAsync(int id);
    
    Task<IEnumerable<EmployeeEntity>> GetEnumerableByCompanyIdAsync(int companyId, int offset, int limit);

    Task<IEnumerable<EmployeeEntity>> GetEnumerableByDepartmentIdAsync(int departmentId, int offset, int limit);
    
    Task<EmployeeEntity> InsertAsync(EmployeeEntity employeeEntity);

    Task<EmployeeEntity> UpdateAsync(EmployeeEntity employeeEntity);
    
    Task<EmployeeEntity> DeleteAsync(int id);
}