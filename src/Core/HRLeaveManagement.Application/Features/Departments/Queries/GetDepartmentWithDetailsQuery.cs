using HRLeaveManagement.Application.DTOs.Departments;
using MediatR;

namespace HRLeaveManagement.Application.Features.Departments.Queries;

public sealed record GetDepartmentWithDetailsQuery(int DepartmentId) : IRequest<DepartmentDetailsDTO>;
