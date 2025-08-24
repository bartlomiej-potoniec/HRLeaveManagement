using MediatR;

namespace HRLeaveManagement.Application.Features.Section.Commands;

public sealed record CreateSectionCommand(string Name,
                                          string? Description,
                                          int DepartmentId,
                                          Guid LeaderId)
    : IRequest<int>;
