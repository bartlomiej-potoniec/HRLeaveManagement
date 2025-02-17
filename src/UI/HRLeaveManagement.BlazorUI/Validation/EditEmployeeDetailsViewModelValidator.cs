using FluentValidation;
using HRLeaveManagement.BlazorUI.ViewModels;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EditEmployeeDetailsViewModelValidator : AbstractValidator<EditEmployeeDetailsViewModel>
{
    public EditEmployeeDetailsViewModelValidator()
    {
        RuleFor(x => x.Position)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Responsibilities)
            .NotNull()
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.SectionId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.LeaderId)
            .NotNull()
            .NotEmpty();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<EditEmployeeDetailsViewModel>
            .CreateWithOptions((EditEmployeeDetailsViewModel)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
            return Array.Empty<string>();

        return result.Errors.Select(e => e.ErrorMessage);
    };

}
