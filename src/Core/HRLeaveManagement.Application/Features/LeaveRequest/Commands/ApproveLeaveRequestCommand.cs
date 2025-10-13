using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands;

public sealed record ApproveLeaveRequestCommand(int Id) : IRequest;
