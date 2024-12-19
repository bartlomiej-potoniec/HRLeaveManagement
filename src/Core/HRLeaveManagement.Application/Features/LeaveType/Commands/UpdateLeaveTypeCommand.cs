using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands;

public sealed record UpdateLeaveTypeCommand(int Id,
                                            string Name,
                                            string? Description,
                                            decimal PaidFraction) 
    : IRequest;
