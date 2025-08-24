using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Domain.Enums;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record CreateEmployeeContractCommand(Guid EmployeeId,
                                                   ContractType ContractType,
                                                   string? ContractDetails, // new
                                                   DateTime StartedAt,
                                                   DateTime? ExpiredAt = null,
                                                   IEnumerable<EmployeeDocumentRequest>? EmployeeDocuments = null /* new */)
    : IRequest<int>;
