using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.LeaveType.UpdateLeaveType.Commands;

namespace HRLeaveManagement.Application.Features.LeaveType.UpdateLeaveType.Validation;

public sealed class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
{
    public UpdateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await leaveTypeRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Leave type with given ID already exists");

        RuleFor(c => c.Name)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty")
            .MaximumLength(70)
                .WithMessage("{PropertyName} must be less than 70");

        RuleFor(c => c.Description)
            .MaximumLength(100)
                .WithMessage("{PropertyName} must be less than 100");

        RuleFor(c => c.PaidFraction)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} must be greather than or equal to 0.0")
            .LessThanOrEqualTo(1)
                .WithMessage("{PropertyName} must be less than or equal to 1.0");

        RuleFor(c => c)
            .MustAsync(async (command, token) => await leaveTypeRepository.IsLeaveTypeUniqueAsync(command.Name, token))
                .WithMessage("Leave type with given name already exists");
    }
}
