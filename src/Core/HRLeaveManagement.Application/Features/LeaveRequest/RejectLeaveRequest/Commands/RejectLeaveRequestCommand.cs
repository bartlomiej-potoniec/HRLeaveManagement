using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.RejectLeaveRequest.Commands;

public sealed record RejectLeaveRequestCommand(int Id) : IRequest;
