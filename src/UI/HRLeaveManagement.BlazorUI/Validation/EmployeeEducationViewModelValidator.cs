using FluentValidation;
using HRLeaveManagement.BlazorUI.ViewModels;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EmployeeEducationViewModelValidator : AbstractValidator<EmployeeEducationViewModel>
{
    public EmployeeEducationViewModelValidator()
    {
        RuleFor(x => x.EducationType)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.EducationDetails)
            .NotNull()
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.EnrolledAt)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.EnrolledAt)
            .LessThan(x => x.GraduatedAt)
                .When(x => x.GraduatedAt.HasValue);

        RuleFor(x => x.GraduatedAt)
            .LessThanOrEqualTo(DateTime.Now)
                .When(x => x.GraduatedAt.HasValue);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<EmployeeEducationViewModel>.CreateWithOptions((EmployeeEducationViewModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
