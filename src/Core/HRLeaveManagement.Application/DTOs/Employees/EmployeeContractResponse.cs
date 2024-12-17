using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeContractResponse(int Id,
                                       ContractType ContractType,
                                       DateOnly EmployeedFrom,
                                       DateOnly? EmployeedTo = null);