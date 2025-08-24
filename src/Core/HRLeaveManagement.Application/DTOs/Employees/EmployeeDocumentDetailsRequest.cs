namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeDocumentDetailsRequest(int? Id,
                                             string Title,
                                             string DocumentNumber,
                                             string FileUrl,
                                             string? Description);
