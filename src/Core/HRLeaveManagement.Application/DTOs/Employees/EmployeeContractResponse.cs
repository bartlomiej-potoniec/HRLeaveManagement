using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeContractResponse(int Id,
                                       ContractType ContractType,
                                       DateTime EmployeedFrom,
                                       DateTime? EmployeedTo = null);