using HRLeaveManagement.Application.DTOs.Sections;
using MediatR;

namespace HRLeaveManagement.Application.Features.Sections.Queries;

public sealed record GetSectionWithDetailsQuery(int Id) : IRequest<SectionDetailsDTO>;
