using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.LeaveType.Commands;

namespace HRLeaveManagement.Application.Validation;

public sealed class DeleteLeaveTypeCommandValidator : AbstractValidator<DeleteLeaveTypeCommand>
{
    public DeleteLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await leaveTypeRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Leave type with given ID does not exists");
    }
}
