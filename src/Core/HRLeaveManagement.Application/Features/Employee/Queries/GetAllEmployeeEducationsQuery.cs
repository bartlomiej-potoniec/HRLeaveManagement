using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Queries;

public sealed record GetAllEmployeeEducationsQuery(Guid EmployeeId) : IRequest<IEnumerable<EmployeeEducationDetailsDTO>>;
