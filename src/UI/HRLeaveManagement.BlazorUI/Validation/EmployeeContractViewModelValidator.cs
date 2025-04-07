using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EmployeeContractViewModelValidator 
    : AbstractValidator<EmployeeContractViewModel>, IViewModelValidator<EmployeeContractViewModel>
{
    public EmployeeContractViewModelValidator()
    {
        RuleFor(x => x.ContractType)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.StartedAt)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.ExpiredAt)
            .NotNull()
            .NotEmpty();
    }
}
