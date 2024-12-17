using HRLeaveManagement.Application.DTOs.Sections;
using MediatR;

namespace HRLeaveManagement.Application.Features.Departments.Queries;

public sealed record GetAllSectionsByDepartmentIdQuery(int DepartmentId) 
    : IRequest<IEnumerable<SectionDTO>>;
