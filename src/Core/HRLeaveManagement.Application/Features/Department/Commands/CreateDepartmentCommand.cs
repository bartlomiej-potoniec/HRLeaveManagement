using MediatR;

namespace HRLeaveManagement.Application.Features.Department.Commands;

public sealed record CreateDepartmentCommand(string Name,
                                             string? Description,
                                             Guid? LeaderId)
    : IRequest<int>;