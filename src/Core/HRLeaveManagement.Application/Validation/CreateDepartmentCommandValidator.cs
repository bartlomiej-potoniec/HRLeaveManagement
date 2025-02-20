using HRLeaveManagement.Application.Features.Department.Commands;
using HRLeaveManagement.Application.Contracts.Identity;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator(IUserService userService)
    {
        RuleFor(c => c.Name)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

        RuleFor(c => c.LeaderId)
            .MustAsync(async (id, token) => id is null || await userService.IsUserInManagerRoleByEmployeeId(id.Value))
                .WithMessage("Leader for given ID does not exist");
    }
}
