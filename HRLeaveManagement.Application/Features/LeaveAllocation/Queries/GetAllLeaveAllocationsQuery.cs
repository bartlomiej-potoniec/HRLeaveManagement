using HRLeaveManagement.Application.DTOs;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Queries;

public sealed record GetAllLeaveAllocationsQuery : IRequest<IEnumerable<LeaveAllocationDTO>>;
