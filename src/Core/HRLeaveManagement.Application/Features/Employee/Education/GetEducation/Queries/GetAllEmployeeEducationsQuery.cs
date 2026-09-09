using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Education.GetEducation.Queries;

public sealed record GetAllEmployeeEducationsQuery(Guid EmployeeId) : IRequest<IEnumerable<EmployeeEducationDetailsDTO>>;
