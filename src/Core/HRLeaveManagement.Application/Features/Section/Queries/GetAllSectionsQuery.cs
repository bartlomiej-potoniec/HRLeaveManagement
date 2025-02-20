using HRLeaveManagement.Application.DTOs.Sections;
using MediatR;

namespace HRLeaveManagement.Application.Features.Section.Queries;

public sealed record GetAllSectionsQuery : IRequest<IEnumerable<SectionDTO>>;
