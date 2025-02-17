using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeEducationRequest(EducationType EducationType,
                                       string EducationDetails,
                                       DateTime EnrolledAt,
                                       DateTime? GraduatedAt = null);
