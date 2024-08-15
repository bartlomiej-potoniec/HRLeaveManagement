using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestCommandValidator(ILeaveTypeRepository leaveTypeRepository,
                                              ILeaveAllocationRepository leaveAllocationRepository,
                                              string employeeId)
    {
        RuleFor(c => c.LeaveTypeId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
            .MustAsync(async (id, token) => await leaveTypeRepository.GetByIdAsync(id) is not null)
                .WithMessage("Leave request does not exist")
            .MustAsync(async (id, token) => await leaveAllocationRepository.GetUserLeaveAllocationsByIdAsync(employeeId, id) is not null)
                .WithMessage("No user leave allocation for Leave Type with id: {PropertyName}");

        RuleFor(c => c.RequestComment)
            .MinimumLength(1)
                .WithMessage("{PropertyName} must be at least {MinLength} length")
            .MaximumLength(1000)
                .WithMessage("{PropertyName} can be maximum {MaxLength} length");

        RuleFor(c => c.StartedAt)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("{PropertyName} must be at least {ComparisonValue}");

        RuleFor(c => c.EndedAt)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThanOrEqualTo(c => c.StartedAt)
                .WithMessage("{PropertyName} must be at least {ComparisonValue}");

        RuleFor(c => c)
            .CustomAsync(async (command, context, token) =>
            {
                var allocation = await leaveAllocationRepository.GetUserLeaveAllocationsByIdAsync(employeeId, command.LeaveTypeId);
                int daysRequested = (int)(command.EndedAt - command.StartedAt).TotalDays;

                if (daysRequested > allocation?.NumberOfDays)
                    context.AddFailure("No days enough for this leave request");
            });
    }
}
