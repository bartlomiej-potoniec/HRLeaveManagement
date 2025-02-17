using HRLeaveManagement.BlazorUI.ViewModels;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EmployeeDetailsViewModelValidator : AbstractValidator<CreateEmployeeDetailsViewModel>
{
    public EmployeeDetailsViewModelValidator()
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

        RuleFor(x => x.Contract.ContractType)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Contract.EmployedFrom)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Contract.EmployedFrom)
            .LessThan(x => x.Contract.EmployedTo)
                .When(x => x.Contract.EmployedTo.HasValue);

        RuleFor(x => x.Contract.EmployedTo)
            .GreaterThan(x => x.Contract.EmployedFrom)
                .When(x => x.Contract.EmployedTo.HasValue);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CreateEmployeeDetailsViewModel>
            .CreateWithOptions((CreateEmployeeDetailsViewModel)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
            return Array.Empty<string>();

        return result.Errors.Select(e => e.ErrorMessage);
    };
} 
