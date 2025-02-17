using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeExperienceResponse(int Id,
                                         ContractType ContractType,
                                         string PreviousCompanyName,
                                         string Position,
                                         DateOnly EmployedFrom,
                                         DateOnly EmployedTo);