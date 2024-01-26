using Microsoft.AspNetCore.Mvc;
using TestAppSmartWay.Application.Requests;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Domain.Responses;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;
using TestAppSmartWay.WebApi.Extensions;

namespace TestAppSmartWay.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class CompanyController(ICompanyRepository companyRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
    {
        var company = new CompanyEntity(request.Name);

        var insertCompanyResult = await companyRepository.InsertAsync(company);

        return new Result<int>(insertCompanyResult.Id).ToActionResult();
    }
}