using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands;

public sealed record UpdateLeaveAllocationCommand(int Id,
                                                  int NumberOfDays,
                                                  int LeaveTypeId,
                                                  int Period) : IRequest;
