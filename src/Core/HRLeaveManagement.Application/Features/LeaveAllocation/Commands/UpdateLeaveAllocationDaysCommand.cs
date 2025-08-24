using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands;

public sealed record UpdateLeaveAllocationDaysCommand(int Id, int? AvailableDays) : IRequest;
