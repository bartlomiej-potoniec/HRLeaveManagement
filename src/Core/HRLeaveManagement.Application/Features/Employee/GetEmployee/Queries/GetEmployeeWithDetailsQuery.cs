using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.GetEmployee.Queries;

public sealed record GetEmployeeWithDetailsQuery(Guid Id) : IRequest<EmployeeDetailsDTO>;
