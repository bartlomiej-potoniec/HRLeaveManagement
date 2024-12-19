using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands;

public sealed record CreateLeaveTypeCommand(string Name,
                                            string? Description,
                                            decimal PaidFraction) 
    : IRequest<int>;
