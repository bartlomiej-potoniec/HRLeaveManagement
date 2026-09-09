using FluentValidation;
using HRLeaveManagement.Application.Features.LeaveRequest.ApproveLeaveRequest.Commands;

namespace HRLeaveManagement.Application.Features.LeaveRequest.ApproveLeaveRequest.Validation;

public sealed class ApproveLeaveRequestCommandValidator : AbstractValidator<ApproveLeaveRequestCommand>
{
    public ApproveLeaveRequestCommandValidator()
    {
        
    }
}
