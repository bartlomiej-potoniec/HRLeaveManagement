using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record CreateEmployeeExperienceCommand(Guid EmployeeId) 
    : IRequest<Guid>;
