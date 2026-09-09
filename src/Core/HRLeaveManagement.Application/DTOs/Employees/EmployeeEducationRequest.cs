using HRLeaveManagement.Domain.Employee.Education;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeEducationRequest(EducationType EducationType,
                                       string InstitutionName, // new
                                       DateTime EnrolledAt,
                                       DateTime? GraduatedAt = null,
                                       string? EducationDetails = null,
                                       IEnumerable<EmployeeDocumentRequest>? EmployeeDocuments = null /* new */);
