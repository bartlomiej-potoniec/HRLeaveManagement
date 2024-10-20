using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands;

public sealed record CreateLeaveRequestCommand(int LeaveTypeId,
                                               DateTime StartedAt,
                                               DateTime EndedAt,
                                               string? RequestComment) 
    : IRequest<int>;
