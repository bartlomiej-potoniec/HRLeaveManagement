using HRLeaveManagement.Application.Features.Department.Commands;
using HRLeaveManagement.Application.Contracts.Identity;
using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Validation;

public sealed class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator(IUserService userService,
                                            IDepartmentRepository departmentRepository)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await departmentRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Department for given ID does not exist");

        RuleFor(c => c.Name)
           .NotNull()
               .WithMessage("{PropertyName} is required")
           .NotEmpty()
               .WithMessage("{PropertyName} cannot be empty");

        RuleFor(c => c.LeaderId)
            .MustAsync(async (id, token) => id is null || await userService.IsUserInManagerRoleByEmployeeIdAsync(id.Value, token))
                .WithMessage("Leader for given ID does not exist");
    }
}
