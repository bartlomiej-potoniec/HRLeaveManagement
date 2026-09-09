using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.LeaveAllocation.UpdateLeaveAllocation.Commands;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.UpdateLeaveAllocation.Validation;

public sealed class UpdateLeaveAllocationCommandValidator : AbstractValidator<UpdateLeaveAllocationDaysCommand>
{
    public UpdateLeaveAllocationCommandValidator(ILeaveAllocationRepository leaveAllocationRepository)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
            .MustAsync(async (id, token) => await leaveAllocationRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Leave allocation does not exist");

        RuleFor(c => c.AvailableDays)
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0");
    }
}
