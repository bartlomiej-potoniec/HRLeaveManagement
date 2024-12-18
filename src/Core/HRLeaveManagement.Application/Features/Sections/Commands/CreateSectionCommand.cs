using MediatR;

namespace HRLeaveManagement.Application.Features.Sections.Commands;

public sealed record CreateSectionCommand(string Name,
                                          string? Description,
                                          int DepartmentId,
                                          Guid? LeaderId)
    : IRequest<int>;
