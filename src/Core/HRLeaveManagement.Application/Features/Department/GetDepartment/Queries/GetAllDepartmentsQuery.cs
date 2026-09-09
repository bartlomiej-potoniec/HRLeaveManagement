using HRLeaveManagement.Application.DTOs.Departments;
using MediatR;

namespace HRLeaveManagement.Application.Features.Department.GetDepartment.Queries;

public sealed record GetAllDepartmentsQuery : IRequest<IEnumerable<DepartmentDTO>>;
