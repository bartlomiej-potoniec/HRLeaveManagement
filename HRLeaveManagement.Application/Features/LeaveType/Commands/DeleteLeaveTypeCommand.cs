using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands;

public sealed record DeleteLeaveTypeCommand(int Id) : IRequest;
