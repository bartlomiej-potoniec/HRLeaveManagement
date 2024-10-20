using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands;

namespace HRLeaveManagement.Application.Validation;

public sealed class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
{
    public UpdateLeaveTypeCommandValidator(ILeaveTypeRepository repository)
    {
        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(70)
                .WithMessage("{PropertyName} must be fewer than 70");

        RuleFor(c => c.DefaultDays)
            .LessThan(100)
                .WithMessage("{PropertyName} cannot exceed 100")
            .GreaterThan(1)
                .WithMessage("{PropertyName} cannot be less than 1");

        RuleFor(c => c.Id)
            .MustAsync(async (id, token) => await repository.GetByIdAsync(id) is not null)
                .WithMessage("Leave type does not exists");
    }
}
