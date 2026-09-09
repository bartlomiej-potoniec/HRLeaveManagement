using HRLeaveManagement.Application.DTOs.Sections;
using MediatR;

namespace HRLeaveManagement.Application.Features.Department.Section.GetSection.Queries;

public sealed record GetSectionWithDetailsQuery(int Id) : IRequest<SectionDetailsDTO>;
