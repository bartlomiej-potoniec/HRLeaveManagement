using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.UpdateLeaveType.Commands;

public sealed record UpdateLeaveTypeCommand(int Id,
                                            string Name,
                                            string? Description,
                                            decimal PaidFraction) 
    : IRequest;
