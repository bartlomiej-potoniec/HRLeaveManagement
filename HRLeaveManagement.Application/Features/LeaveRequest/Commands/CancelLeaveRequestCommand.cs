using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands;

public sealed record CancelLeaveRequestCommand(int Id) : IRequest;
