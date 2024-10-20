using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands;

public sealed record UpdateLeaveRequestCommand(int Id,
                                               string RequestedEmployeeId,
                                               DateTime StartedAt,
                                               DateTime EndedAt,
                                               string RequestComment,
                                               bool IsCanceled) : IRequest;