using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestCommandValidator(DateTime todaysDate)
    {
        RuleFor(c => c.RequesterComment)
            .MinimumLength(1)
                .WithMessage("{PropertyName} must be at least {MinLength} length")
            .MaximumLength(1000)
                .WithMessage("{PropertyName} can be maximum {MaxLength} length");

        RuleFor(c => c.StartedAt)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThanOrEqualTo(todaysDate)
                .WithMessage("{PropertyName} must be at least {ComparisonValue}");

        RuleFor(c => c.EndedAt)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThanOrEqualTo(c => c.StartedAt)
                .WithMessage("{PropertyName} must be at least {ComparisonValue}");
    }
}
