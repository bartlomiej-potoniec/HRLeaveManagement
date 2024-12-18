using HRLeaveManagement.Application.DTOs.Sections;
using MediatR;

namespace HRLeaveManagement.Application.Features.Sections.Queries;

public sealed record GetAllSectionsQuery : IRequest<IEnumerable<SectionDTO>>;
