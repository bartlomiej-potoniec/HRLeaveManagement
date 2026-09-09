using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Contract.GetContract.Queries;

public sealed record GetAllEmployeeContractsQuery(Guid EmployeeId) : IRequest<IEnumerable<EmployeeContractDetailsDTO>>;
