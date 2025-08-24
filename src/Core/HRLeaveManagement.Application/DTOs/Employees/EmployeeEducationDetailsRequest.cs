using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public sealed record EmployeeEducationDetailsRequest(int? Id,
                                                     EducationType EducationType,
                                                     string InstitutionName, // new
                                                     string EducationDetails,
                                                     DateTime EnrolledAt,
                                                     DateTime? GraduatedAt = null,
                                                     IEnumerable<EmployeeDocumentDetailsRequest> EmployeeDocuments = null);
