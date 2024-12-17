using MediatR;

namespace HRLeaveManagement.Application.Features.Departments.Commands;

public sealed record CreateDepartmentCommand(string Name,
                                             string? Description,
                                             Guid? LeaderId)
    : IRequest<int>;