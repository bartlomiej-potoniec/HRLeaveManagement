using MediatR;

namespace HRLeaveManagement.Application.Features.Department.Commands;

public sealed record UpdateDepartmentCommand(int Id,
                                             string Name,
                                             string? Description,
                                             Guid? LeaderId) 
    : IRequest;
