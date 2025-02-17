using FluentValidation;
using HRLeaveManagement.BlazorUI.ViewModels;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EmployeeContractViewModelValidator : AbstractValidator<EmployeeContractViewModel>
{
    public EmployeeContractViewModelValidator()
    {
        RuleFor(x => x.ContractType)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.EmployedFrom)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.EmployedTo)
            .NotNull()
            .NotEmpty();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<EmployeeContractViewModel>
            .CreateWithOptions((EmployeeContractViewModel)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
            return Array.Empty<string>();

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
