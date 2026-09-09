using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Experience.CreateExperieence.Commands;

public sealed record CreateEmployeeExperienceCommand(Guid EmployeeId) 
    : IRequest<Guid>;
