using TestAppSmartWay.Domain.Entities;

namespace TestAppSmartWay.Infrastructure.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<DepartmentEntity?> GetByIdAsync(int id);
    
    Task<DepartmentEntity> InsertAsync(DepartmentEntity departmentEntity);
}