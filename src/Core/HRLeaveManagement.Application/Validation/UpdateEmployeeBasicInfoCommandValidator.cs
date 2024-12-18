using FluentValidation;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.Employee.Commands;

namespace HRLeaveManagement.Application.Validation;

public sealed class UpdateEmployeeBasicInfoCommandValidator : AbstractValidator<UpdateEmployeeBasicInfoCommand>
{
    public UpdateEmployeeBasicInfoCommandValidator(IEmployeeRepository employeeRepository,
                                                   ISectionRepository sectionRepository,
                                                   IUserService userService)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await employeeRepository.GetByIdAsync(id) is not null)
                .WithMessage("Employee for given ID does not exist");

        RuleFor(c => c.Position)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

        RuleFor(c => c.Responsibilities)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

        RuleFor(c => c.SectionId)
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
            .MustAsync(async (id, token) => id is null || await sectionRepository.GetByIdAsync(id.Value) is not null)
                .WithMessage("Section for given ID does not exist");

        RuleFor(c => c.LeaderId)
            .MustAsync(async (id, token) => id is null || await userService.IsUserInManagerRoleByEmployeeId(id.Value))
                .WithMessage("Leader for given ID does not exist");
    }
}
