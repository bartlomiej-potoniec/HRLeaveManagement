using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Queries;

public sealed record GetEmployeeEducationWithDetailsQuery(Guid EmployeeId, int EducationId) 
    : IRequest<EmployeeEducationDetailsDTO>;
