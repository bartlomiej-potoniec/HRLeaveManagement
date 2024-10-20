using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateLeaveAllocationCommandValidator : AbstractValidator<CreateLeaveAllocationCommand>
{
    public CreateLeaveAllocationCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(c => c.LeaveTypeId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
            .MustAsync(async (id, token) => await leaveTypeRepository.GetByIdAsync(id) is not null)
                .WithMessage("Leave type with ID: {PropertyValue} does not exist");

        // TODO: Add validation for employee (if employee id exists and current user id is employee id)
    }
}
