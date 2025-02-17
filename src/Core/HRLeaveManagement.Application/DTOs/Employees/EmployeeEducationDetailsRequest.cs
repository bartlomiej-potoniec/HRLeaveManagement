using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public sealed record EmployeeEducationDetailsRequest(int? Id,
                                                     EducationType EducationType,
                                                     string EducationDetails,
                                                     DateTime EnrolledAt,
                                                     DateTime? GraduatedAt = null);
