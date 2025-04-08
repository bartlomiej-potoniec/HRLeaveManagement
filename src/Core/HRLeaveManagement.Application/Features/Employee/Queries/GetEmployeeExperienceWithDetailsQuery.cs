using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Queries;

public sealed record GetEmployeeExperienceWithDetailsQuery(Guid EmployeeId, int ExperienceId)
    : IRequest<EmployeeExperienceDetailsDTO>;
