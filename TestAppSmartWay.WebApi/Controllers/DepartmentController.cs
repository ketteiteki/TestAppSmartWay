using Microsoft.AspNetCore.Mvc;
using TestAppSmartWay.Application.Requests;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;

namespace TestAppSmartWay.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class DepartmentController(IDepartmentRepository departmentRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequest request)
    {
        var department = DepartmentEntity.Create(request.Name, request.Phone);

        var insertDepartmentResult = await departmentRepository.InsertAsync(department);

        return Ok(insertDepartmentResult.Id);
    }
}