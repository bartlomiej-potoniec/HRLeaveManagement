using HRLeaveManagement.Application.DTOs.Sections;
using MediatR;

namespace HRLeaveManagement.Application.Features.Department.Section.GetSection.Queries;

public sealed record GetAllSectionsQuery : IRequest<IEnumerable<SectionDTO>>;
