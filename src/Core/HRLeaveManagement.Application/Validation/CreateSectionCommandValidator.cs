using HRLeaveManagement.Application.Features.Section.Commands;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Identity;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateSectionCommandValidator : AbstractValidator<CreateSectionCommand>
{
    public CreateSectionCommandValidator(IUserService userService,
                                         IDepartmentRepository departmentRepository)
    {
        RuleFor(c => c.Name)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

        RuleFor(c => c.DepartmentId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await departmentRepository.GetByIdAsync(id) is not null)
                .WithMessage("Department for given ID does not exist");

        RuleFor(c => c.LeaderId)
            .MustAsync(async (id, token) => id is null || await userService.IsUserInManagerRoleByEmployeeIdAsync(id.Value, token))
                .WithMessage("Leader for given ID does not exist");
    }
}
