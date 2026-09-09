using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CancelLeaveRequest.Commands;

public sealed record CancelLeaveRequestCommand(int Id) : IRequest;
