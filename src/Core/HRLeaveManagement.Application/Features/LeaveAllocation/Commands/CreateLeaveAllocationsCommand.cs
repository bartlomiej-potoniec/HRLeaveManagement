using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands;

public sealed record CreateLeaveAllocationsCommand(int LeaveTypeId, string? LeaveRuleSet) : IRequest;
