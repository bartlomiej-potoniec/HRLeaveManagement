using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands;

public sealed record ChangeLeaveRequestApprovalCommand(int Id, bool IsApproved) : IRequest;
