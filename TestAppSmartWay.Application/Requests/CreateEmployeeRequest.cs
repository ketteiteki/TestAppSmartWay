namespace TestAppSmartWay.Application.Requests;

public record CreateEmployeeRequest(
    string Name, 
    string Surname,
    string Phone, 
    int CompanyId, 
    int PassportId,
    int DepartmentId);