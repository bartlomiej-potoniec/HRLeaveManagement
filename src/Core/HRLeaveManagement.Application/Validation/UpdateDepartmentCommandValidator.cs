using HRLeaveManagement.Application.Features.Department.Commands;
using HRLeaveManagement.Application.Contracts.Identity;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator(IUserService userService)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required");

        RuleFor(c => c.Name)
           .NotNull()
               .WithMessage("{PropertyName} is required")
           .NotEmpty()
               .WithMessage("{PropertyName} cannot be empty");

        RuleFor(c => c.LeaderId)
            .MustAsync(userService.IsUserInManagerRoleByEmployeeIdAsync)
                .WithMessage("Leader for given ID does not exist");
    }
}
