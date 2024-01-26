using TestAppSmartWay.Domain.Entities;

namespace TestAppSmartWay.Infrastructure.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task<CompanyEntity?> GetByIdAsync(int id);
    
    Task<CompanyEntity> InsertAsync(CompanyEntity companyEntity);
}