using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TestAppSmartWay.Application.BusinessLogic;
using TestAppSmartWay.Application.Requests;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;
using TestAppSmartWay.WebApi.Extensions;

namespace TestAppSmartWay.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployeeController(
    IEmployeeRepository employeeRepository, 
    EmployeeService employeeService) : ControllerBase
{
    [HttpGet("Company/{id:int}")]
    public async Task<IActionResult> GetEmployeesByCompanyId(int id, [FromQuery] int offset, [FromQuery] int limit)
    {
        var employees = await employeeRepository.GetEnumerableByCompanyIdAsync(id, offset, limit);

        return Ok(employees);
    }
    
    [HttpGet("Department/{id:int}")]
    public async Task<IActionResult> GetEmployeesByDepartmentId(int id, [FromQuery] int offset, [FromQuery] int limit)
    {
        var employees = await employeeRepository.GetEnumerableByDepartmentIdAsync(id, offset, limit);

        return Ok(employees);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
    {
        var result = await employeeService.CreateEmployeeAsync(
            request.Name,
            request.Surname,
            request.Phone,
            request.CompanyId,
            request.PassportId,
            request.DepartmentId);

        return result.ToActionResult();
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Dictionary<string, JsonElement> dictionary)
    {
        var result = await employeeService.UpdateEmployeeAsync(id, dictionary);

        return result.ToActionResult();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await employeeService.DeleteEmployeeAsync(id);

        return result.ToActionResult();
    }
}