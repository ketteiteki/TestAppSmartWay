using Microsoft.AspNetCore.Mvc;
using TestAppSmartWay.Application.Requests;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Domain.Responses;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;
using TestAppSmartWay.WebApi.Extensions;

namespace TestAppSmartWay.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class PassportController(IPassportRepository passportRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePassport([FromBody] CreatePassportRequest request)
    {
        var passport = new PassportEntity(request.Type, request.Number);

        var insertPassportResult = await passportRepository.InsertAsync(passport);
        
        return new Result<int>(insertPassportResult.Id).ToActionResult();
    }
}