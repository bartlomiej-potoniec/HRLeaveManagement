using HRLeaveManagement.Domain.Employee.Contract;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Contract.UpdateContract.Commands;

public sealed record UpdateEmployeeContractCommand(Guid EmployeeId,
                                                   int ContractId,
                                                   ContractType ContractType,
                                                   DateTime EmployeedFrom,
                                                   DateTime? EmployeedTo = null)
    : IRequest;
