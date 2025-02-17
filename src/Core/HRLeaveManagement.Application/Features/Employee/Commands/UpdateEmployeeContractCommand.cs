using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record UpdateEmployeeContractCommand(Guid EmployeeId,
                                                   int ContractId,
                                                   ContractType ContractType,
                                                   DateTime EmployeedFrom,
                                                   DateTime? EmployeedTo = null);
