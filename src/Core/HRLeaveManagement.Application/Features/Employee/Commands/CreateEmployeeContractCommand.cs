using HRLeaveManagement.Domain.Enums;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record CreateEmployeeContractCommand(Guid EmployeeId,
                                                   ContractType ContractType,
                                                   DateOnly EmployeedFrom,
                                                   DateOnly? EmployeedTo = null)
    : IRequest<int>;
