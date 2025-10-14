using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands;

public sealed record RejectLeaveRequestCommand(int Id) : IRequest;
