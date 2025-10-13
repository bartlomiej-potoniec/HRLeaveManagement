using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class CancelLeaveRequestCommandValidator : AbstractValidator<CancelLeaveRequestCommand>
{
    public CancelLeaveRequestCommandValidator()
    {
        
    }
}
