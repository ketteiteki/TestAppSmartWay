using Microsoft.AspNetCore.Mvc;
using TestAppSmartWay.Application.Requests;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Domain.Responses;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;
using TestAppSmartWay.WebApi.Extensions;

namespace TestAppSmartWay.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class DepartmentController(IDepartmentRepository departmentRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequest request)
    {
        var department = new DepartmentEntity(request.Name, request.Phone);

        var insertDepartmentResult = await departmentRepository.InsertAsync(department);

        return new Result<int>(insertDepartmentResult.Id).ToActionResult();
    }
}