using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeContractRequest(ContractType ContractType,
                                      DateTime EmployeedFrom,
                                      DateTime? EmployeedTo = null);
