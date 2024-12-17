using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeContractRequest(ContractType ContractType,
                                      DateOnly EmployeedFrom,
                                      DateOnly? EmployeedTo = null);