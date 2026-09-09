using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Experience.GetExperience.Queries;

public sealed record GetAllEmployeeExperiencesQuery(Guid EmployeeId) : IRequest<IEnumerable<EmployeeExperienceDetailsDTO>>;