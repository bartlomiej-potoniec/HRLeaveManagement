using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Contract.GetContract.Queries;

public sealed record GetEmployeeContractWithDetailsQuery(Guid EmployeeId, int ContractId) 
    : IRequest<EmployeeContractDetailsDTO>;