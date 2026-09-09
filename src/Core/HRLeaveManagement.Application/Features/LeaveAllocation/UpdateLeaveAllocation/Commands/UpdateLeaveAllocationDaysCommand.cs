using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.UpdateLeaveAllocation.Commands;

public sealed record UpdateLeaveAllocationDaysCommand(int Id, int? AvailableDays) : IRequest;
