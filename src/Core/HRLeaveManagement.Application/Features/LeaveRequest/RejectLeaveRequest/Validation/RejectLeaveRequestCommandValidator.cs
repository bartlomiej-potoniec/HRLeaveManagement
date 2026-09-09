using FluentValidation;
using HRLeaveManagement.Application.Features.LeaveRequest.RejectLeaveRequest.Commands;

namespace HRLeaveManagement.Application.Features.LeaveRequest.RejectLeaveRequest.Validation;

public sealed class RejectLeaveRequestCommandValidator : AbstractValidator<RejectLeaveRequestCommand>
{
    public RejectLeaveRequestCommandValidator()
    {
        
    }
}
