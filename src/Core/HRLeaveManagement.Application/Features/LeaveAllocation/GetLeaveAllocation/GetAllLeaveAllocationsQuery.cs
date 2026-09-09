using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.GetLeaveAllocation;

public sealed record GetAllLeaveAllocationsQuery : IRequest<IEnumerable<LeaveAllocationDTO>>;
