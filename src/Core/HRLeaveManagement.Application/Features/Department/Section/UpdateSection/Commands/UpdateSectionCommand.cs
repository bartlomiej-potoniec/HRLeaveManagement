using MediatR;

namespace HRLeaveManagement.Application.Features.Department.Section.UpdateSection.Commands;

public sealed record UpdateSectionCommand(int Id,
                                          string Name,
                                          string? Description,
                                          int DepartmentId,
                                          Guid LeaderId)
    : IRequest;