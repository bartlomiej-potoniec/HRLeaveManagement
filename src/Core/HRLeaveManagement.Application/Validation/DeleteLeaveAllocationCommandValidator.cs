using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;

namespace HRLeaveManagement.Application.Validation;

public sealed class DeleteLeaveAllocationCommandValidator : AbstractValidator<DeleteLeaveAllocationCommand>
{
    public DeleteLeaveAllocationCommandValidator(ILeaveAllocationRepository repository)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0");
    }
}
