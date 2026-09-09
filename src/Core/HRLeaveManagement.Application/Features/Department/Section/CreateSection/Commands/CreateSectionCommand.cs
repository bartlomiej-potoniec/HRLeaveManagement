using MediatR;

namespace HRLeaveManagement.Application.Features.Department.Section.CreateSection.Commands;

public sealed record CreateSectionCommand(string Name,
                                          string? Description,
                                          int DepartmentId,
                                          Guid LeaderId)
    : IRequest<int>;
