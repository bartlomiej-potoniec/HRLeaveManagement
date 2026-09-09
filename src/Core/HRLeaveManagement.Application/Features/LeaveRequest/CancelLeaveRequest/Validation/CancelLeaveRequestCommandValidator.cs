using FluentValidation;
using HRLeaveManagement.Application.Features.LeaveRequest.CancelLeaveRequest.Commands;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CancelLeaveRequest.Validation;

public sealed class CancelLeaveRequestCommandValidator : AbstractValidator<CancelLeaveRequestCommand>
{
    public CancelLeaveRequestCommandValidator()
    {
        
    }
}
