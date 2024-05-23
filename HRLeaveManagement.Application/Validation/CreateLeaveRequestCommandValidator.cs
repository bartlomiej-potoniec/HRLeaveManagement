using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestCommandValidator(ILeaveTypeRepository repository)
    {
        RuleFor(c => c.LeaveTypeId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
            .MustAsync(async (id, token) => await repository.GetByIdAsync(id) is not null)
                .WithMessage("Leave request does not exist");

        RuleFor(c => c.RequestComment)
            .MinimumLength(1)
                .WithMessage("{PropertyName} must be at least {MinLength} length")
            .MaximumLength(1000)
                .WithMessage("{PropertyName} can be maximum {MaxLength} length");

        RuleFor(c => c.StartedAt)
            .GreaterThanOrEqualTo(DateTime.Now)
                .WithMessage("{PropertyName} must be at least {ComparisonValue}");

        RuleFor(c => c.EndedAt)
            .GreaterThanOrEqualTo(c => c.StartedAt)
                .WithMessage("{PropertyName} must be at least {ComparisonValue}");

        // TODO: Add validation for employee id
    }
}
