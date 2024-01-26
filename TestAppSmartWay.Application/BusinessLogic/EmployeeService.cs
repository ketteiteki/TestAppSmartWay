using System.Text.Json;
using TestAppSmartWay.Domain.Constants;
using TestAppSmartWay.Domain.Entities;
using TestAppSmartWay.Domain.Responses;
using TestAppSmartWay.Domain.Responses.Errors;
using TestAppSmartWay.Infrastructure.Repositories.Interfaces;

namespace TestAppSmartWay.Application.BusinessLogic;

public class EmployeeService(
    IPassportRepository passportRepository, 
    ICompanyRepository companyRepository,
    IDepartmentRepository departmentRepository,
    IEmployeeRepository employeeRepository)
{
    public async Task<Result<int>> CreateEmployeeAsync(string name, string surname, string phone, int companyId, 
        int passportId, int departmentId)
    {
        var passport = await passportRepository.GetByIdAsync(passportId);
        var company = await companyRepository.GetByIdAsync(companyId);
        var department = await departmentRepository.GetByIdAsync(departmentId);

        if (passport == null)
        {
            return new Result<int>(new Error(ResponseMessages.PassportNotFound));
        }
        
        if (company == null)
        {
            return new Result<int>(new Error(ResponseMessages.CompanyNotFound));
        }
        
        if (department == null)
        {
            return new Result<int>(new Error(ResponseMessages.DepartmentNotFound));
        }
        
        var employee = EmployeeEntity.Create(name, surname, phone, companyId, passport, department);

        var insertedEmployee = await employeeRepository.InsertAsync(employee);
        
        return new Result<int>(insertedEmployee.Id);
    }

    public async Task<Result<int>> UpdateEmployeeAsync(int id, Dictionary<string, JsonElement> dictionary)
    {   
        var employee = await employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return new Result<int>(new Error(ResponseMessages.EmployeeNotFound));
        }

        var wasEntityChanged = false;
        
        foreach (var item in dictionary)
        {
            switch (item.Key)
            {
                case nameof(employee.Name):
                    var name = item.Value.GetString();
                    if (name == null) continue;
                    wasEntityChanged = true;
                    employee.UpdateName(name);
                    break;
                case nameof(employee.Surname):
                    var surname = item.Value.GetString();
                    if (surname == null) continue;
                    wasEntityChanged = true;
                    employee.UpdateSurname(surname);
                    break;
                case nameof(employee.Phone): 
                    var phone = item.Value.GetString();
                    if (phone == null) continue;
                    wasEntityChanged = true;
                    employee.UpdatePhone(phone);
                    break;
                case nameof(employee.CompanyId):
                    var companyId = item.Value.GetInt32();
                    if (companyId == default) continue;
                    var company = await companyRepository.GetByIdAsync(companyId);
                    if (company == null)
                    {
                        return new Result<int>(new Error(ResponseMessages.CompanyNotFound));
                    }
                    wasEntityChanged = true;
                    employee.UpdateCompanyId(companyId);
                    break;
                case "PassportId":
                    var passportId = item.Value.GetInt32();
                    if (passportId == default) continue;
                    var passport = await passportRepository.GetByIdAsync(passportId);
                    if (passport == null)
                    {
                        return new Result<int>(new Error(ResponseMessages.PassportNotFound));
                    }
                    wasEntityChanged = true;
                    employee.Passport.UpdateId(passportId);
                    break;
                case "DepartmentId": 
                    var departmentId = item.Value.GetInt32();
                    if (departmentId == default) continue;
                    var department = await passportRepository.GetByIdAsync(departmentId);
                    if (department == null)
                    {
                        return new Result<int>(new Error(ResponseMessages.DepartmentNotFound));
                    }
                    wasEntityChanged = true;
                    employee.Department.UpdateId(departmentId);
                    break;
            }
        }

        if (wasEntityChanged)
        {
            await employeeRepository.UpdateAsync(employee);
        }
        
        return new Result<int>(employee.Id);
    }
    
    public async Task<Result<int>> DeleteEmployeeAsync(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return new Result<int>(new Error(ResponseMessages.EmployeeNotFound));
        }

        await employeeRepository.DeleteAsync(employee.Id);
        
        return new Result<int>(employee.Id);
    }
}