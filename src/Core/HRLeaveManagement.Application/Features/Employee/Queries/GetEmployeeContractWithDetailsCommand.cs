using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Queries;

public sealed record GetEmployeeContractWithDetailsCommand(Guid EmployeeId, int ContractId) 
    : IRequest<EmployeeContractDetailsDTO>;