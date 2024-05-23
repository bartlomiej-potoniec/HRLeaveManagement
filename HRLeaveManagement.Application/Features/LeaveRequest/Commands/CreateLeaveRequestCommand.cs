using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands;

public sealed record CreateLeaveRequestCommand(string RequestedEmployeeId,
                                               int LeaveTypeId,
                                               DateTime StartedAt,
                                               DateTime EndedAt,
                                               string? RequestComment) 
    : IRequest<int>;
