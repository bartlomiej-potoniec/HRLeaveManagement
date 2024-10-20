using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands;

public sealed record DeleteLeaveRequestCommand(int Id) : IRequest;