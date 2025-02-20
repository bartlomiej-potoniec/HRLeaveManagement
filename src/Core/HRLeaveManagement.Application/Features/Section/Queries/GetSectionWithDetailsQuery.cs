using HRLeaveManagement.Application.DTOs.Sections;
using MediatR;

namespace HRLeaveManagement.Application.Features.Section.Queries;

public sealed record GetSectionWithDetailsQuery(int Id) : IRequest<SectionDetailsDTO>;
