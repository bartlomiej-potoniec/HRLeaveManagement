using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Extensions;
using HRLeaveManagement.BlazorUI.ViewModels;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class LoginViewModelValidator 
    : AbstractValidator<LoginViewModel>, IViewModelValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor(x => x.Username)
            .NotNull()
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50)
                .WithDisplayName(x => x.Username);

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50)
                .WithDisplayName(x => x.Password);
    }
}
