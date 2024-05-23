using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;

namespace HRLeaveManagement.Application.Validation;

public sealed class UpdateLeaveAllocationCommandValidator : AbstractValidator<UpdateLeaveAllocationCommand>
{
    public UpdateLeaveAllocationCommandValidator(ILeaveTypeRepository leaveTypeRepository,
                                                 ILeaveAllocationRepository leaveAllocationRepository)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
            .MustAsync(async (id, token) => await leaveAllocationRepository.GetByIdAsync(id) is not null)
                .WithMessage("Leave allocation does not exist");

        RuleFor(c => c.NumberOfDays)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
            .MustAsync(async (command, numOfDays, token) =>
            {
                var leaveType = await leaveTypeRepository.GetByIdAsync(command.LeaveTypeId);
                return numOfDays <= leaveType!.DefaultDays;
            })
                .WithMessage("{PropertyName} cannot be greater than leave type default days number");

        RuleFor(c => c.LeaveTypeId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
            .MustAsync(async (id, token) => await leaveTypeRepository.GetByIdAsync(id) is not null)
                .WithMessage("Leave type does not exist");

        RuleFor(c => c.Period)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("{PropertyName} must be after {ComparisonValue}");
    }
}
