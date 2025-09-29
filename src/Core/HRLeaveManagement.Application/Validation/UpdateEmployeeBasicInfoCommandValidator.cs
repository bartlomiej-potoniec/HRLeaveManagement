using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.Employee.Commands;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class UpdateEmployeeBasicInfoCommandValidator : AbstractValidator<UpdateEmployeeBasicInfoCommand>
{
    public UpdateEmployeeBasicInfoCommandValidator(IEmployeeRepository employeeRepository,
                                                   ISectionRepository sectionRepository,
                                                   IUserService userService)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required");

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
                .WithMessage("{PropertyName} must be greater than 0");
    }
}
