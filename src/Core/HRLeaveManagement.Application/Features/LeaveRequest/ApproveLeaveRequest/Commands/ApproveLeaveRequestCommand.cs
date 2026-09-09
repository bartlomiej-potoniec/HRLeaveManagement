using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.ApproveLeaveRequest.Commands;

public sealed record ApproveLeaveRequestCommand(int Id) : IRequest;
