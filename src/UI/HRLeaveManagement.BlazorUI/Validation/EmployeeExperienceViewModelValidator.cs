using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Extensions;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EmployeeExperienceViewModelValidator 
    : AbstractValidator<EmployeeExperienceViewModel>, IViewModelValidator<EmployeeExperienceViewModel>
{
    public EmployeeExperienceViewModelValidator()
    {
        RuleFor(x => x.PreviousCompanyName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100)
                .WithDisplayName(x => x.PreviousCompanyName);

        RuleFor(x => x.Position)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100)
                .WithDisplayName(x => x.Position);

        RuleFor(x => x.ContractType)
            .NotNull()
            .NotEmpty()
                .WithDisplayName(x => x.ContractType);

        RuleFor(x => x.EmploymentDateRange)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.EmployedFrom)
            .NotNull()
            .NotEmpty()
            .LessThan(x => x.EmployedTo)
                .WithDisplayName(x => x.EmployedFrom);

        RuleFor(x => x.EmployedTo)
            .NotNull()
            .NotEmpty()
            .LessThanOrEqualTo(x => x.MaxEmployedToDate)
                .WithDisplayName(x => x.EmployedTo);
    }
}
