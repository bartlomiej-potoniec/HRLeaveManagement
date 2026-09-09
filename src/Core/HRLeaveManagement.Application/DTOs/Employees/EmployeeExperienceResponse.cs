using HRLeaveManagement.Domain.Employee.Contract;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeExperienceResponse(int Id,
                                         ContractType ContractType,
                                         string PreviousCompanyName,
                                         string Position,
                                         DateOnly EmployedFrom,
                                         DateOnly EmployedTo);