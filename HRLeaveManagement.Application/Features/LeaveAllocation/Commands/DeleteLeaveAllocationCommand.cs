using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands;

public sealed record DeleteLeaveAllocationCommand(int Id) : IRequest;
