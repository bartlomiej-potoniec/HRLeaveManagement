namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeDocumentRequest(string Title,
                                      string DocumentNumber,
                                      string FileUrl,
                                      string? Description);