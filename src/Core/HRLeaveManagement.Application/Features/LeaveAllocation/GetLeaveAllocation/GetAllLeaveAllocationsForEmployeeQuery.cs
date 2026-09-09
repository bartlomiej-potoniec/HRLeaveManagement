using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.GetLeaveAllocation;

public sealed record GetAllLeaveAllocationsForEmployeeQuery(Guid EmployeeId)
    : IRequest<IEnumerable<LeaveAllocationDetailsDTO>>;
