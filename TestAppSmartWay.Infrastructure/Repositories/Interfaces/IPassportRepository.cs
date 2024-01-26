using TestAppSmartWay.Domain.Entities;

namespace TestAppSmartWay.Infrastructure.Repositories.Interfaces;

public interface IPassportRepository
{
    Task<PassportEntity?> GetByIdAsync(int id);
    
    Task<PassportEntity> InsertAsync(PassportEntity passportEntity);
}