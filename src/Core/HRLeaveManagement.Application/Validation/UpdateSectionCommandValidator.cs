using HRLeaveManagement.Application.Features.Section.Commands;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Identity;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class UpdateSectionCommandValidator : AbstractValidator<UpdateSectionCommand>
{
    public UpdateSectionCommandValidator(IUserService userService,
                                         ISectionRepository sectionRepository,
                                         IDepartmentRepository departmentRepository)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await sectionRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Section for given ID does not exist");

        RuleFor(c => c.Name)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

        RuleFor(c => c.DepartmentId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await departmentRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Department for given ID does not exist");

        RuleFor(c => c.LeaderId)
            .MustAsync(async (id, token) => id is null || await userService.IsUserInManagerRoleByEmployeeIdAsync(id.Value, token))
                .WithMessage("Leader for given ID does not exist");
    }
}
