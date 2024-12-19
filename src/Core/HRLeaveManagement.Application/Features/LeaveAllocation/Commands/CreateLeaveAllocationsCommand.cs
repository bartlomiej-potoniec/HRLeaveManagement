using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands;

public sealed record CreateLeaveAllocationsCommand(Guid EmployeeId,
                                                   int Year,
                                                   List<LeaveAllocationForUserRequest> LeaveAllocations) 
    : IRequest;
