using HRLeaveManagement.Application.Contracts.Identity;
using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.Department.Section.UpdateSection.Commands;

namespace HRLeaveManagement.Application.Features.Department.Section.UpdateSection.Validation;

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
