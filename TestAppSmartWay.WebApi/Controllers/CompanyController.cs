using Microsoft.AspNetCore.Mvc;
using TestAppSmartWay.Application.Requests;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;

namespace TestAppSmartWay.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class CompanyController(ICompanyRepository companyRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
    {
        var company = CompanyEntity.Create(request.Name);

        var insertCompanyResult = await companyRepository.InsertAsync(company);

        return Ok(insertCompanyResult.Id);
    }
}