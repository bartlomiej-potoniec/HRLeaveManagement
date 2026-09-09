using HRLeaveManagement.Domain.Employee.Education;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeEducationResponse(int Id,
                                        EducationType EducationType,
                                        string EducationDetails,
                                        DateOnly EnrolledAt,
                                        DateOnly? GraduatedAt = null);