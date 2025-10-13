using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class ApproveLeaveRequestCommandValidator : AbstractValidator<ApproveLeaveRequestCommand>
{
    public ApproveLeaveRequestCommandValidator()
    {
        
    }
}
