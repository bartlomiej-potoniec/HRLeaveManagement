using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record CreateEmployeeEducationCommand(Guid EmployeeId)
    : IRequest<Guid>;