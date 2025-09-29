using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateLeaveAllocationsCommandValidator : AbstractValidator<CreateLeaveAllocationsCommand>
{
    public CreateLeaveAllocationsCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(c => c.LeaveTypeId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await leaveTypeRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Leave type for given ID does not exist")
            .MustAsync(async (id, token) => !(await leaveTypeRepository.GetByIdAsync(id, token))!.IsPredefined)
                .WithMessage("Cannot create custom allocation for predefined leave");
    }
}
