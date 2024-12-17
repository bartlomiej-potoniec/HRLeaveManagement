using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeExperienceResponse(int Id,
                                         ContractType ContractType,
                                         DateOnly EmployedFrom,
                                         DateOnly EmployedTo);