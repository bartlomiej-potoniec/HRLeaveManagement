using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands;

namespace HRLeaveManagement.Application.Validation;

public sealed class DeleteLeaveTypeCommandValidator : AbstractValidator<DeleteLeaveTypeCommand>
{
    public DeleteLeaveTypeCommandValidator(ILeaveTypeRepository repository)
    {
        RuleFor(c => c.Id)
            .MustAsync(async (id, token) => await repository.GetByIdAsync(id) is not null)
                .WithMessage("Leave type does not exists");
    }
}
