using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Queries;

public sealed record GetLeaveAllocationWithDetailsQuery(int Id) : IRequest<LeaveAllocationDetailsDTO>;
