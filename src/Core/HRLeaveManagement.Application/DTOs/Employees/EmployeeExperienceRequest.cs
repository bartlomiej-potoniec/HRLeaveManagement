using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeExperienceRequest(ContractType ContractType,
                                        DateOnly EmployedFrom,
                                        DateOnly EmployedTo);
