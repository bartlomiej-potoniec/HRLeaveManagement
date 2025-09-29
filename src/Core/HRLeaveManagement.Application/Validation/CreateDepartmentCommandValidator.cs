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
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(userService.IsUserInManagerRoleByEmployeeIdAsync)
                .WithMessage("Leader for given ID does not exist");
    }
}
