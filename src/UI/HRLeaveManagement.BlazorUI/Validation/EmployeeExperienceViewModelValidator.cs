using HRLeaveManagement.BlazorUI.ViewModels;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EmployeeExperienceViewModelValidator : AbstractValidator<EmployeeExperienceViewModel>
{
    public EmployeeExperienceViewModelValidator()
    {
        RuleFor(x => x.PreviousCompanyName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Position)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ContractType)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.EmploymentDateRange)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.EmploymentDateRange.End)
            .LessThanOrEqualTo(DateTime.Now);

        RuleFor(x => x.EmploymentDateRange.Start)
            .LessThan(x => x.EmploymentDateRange.End);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<EmployeeExperienceViewModel>.CreateWithOptions((EmployeeExperienceViewModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
