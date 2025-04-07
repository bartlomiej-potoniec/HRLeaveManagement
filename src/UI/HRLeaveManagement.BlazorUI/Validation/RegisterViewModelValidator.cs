using HRLeaveManagement.BlazorUI.ViewModels;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Extensions;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class RegisterViewModelValidator 
    : AbstractValidator<RegisterViewModel>, IViewModelValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50)
                .WithDisplayName(x => x.FirstName);

        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50)
                .WithDisplayName(x => x.LastName);

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
                .WithDisplayName(x => x.Email);

        RuleFor(x => x.DateOfBirth)
            .NotNull()
            .NotEmpty()
            .GreaterThan(new DateTime((DateTime.Now.Year - 100), 1, 1))
            .LessThan(new DateTime((DateTime.Now.Year - 18), 1, 1))
                .WithDisplayName(x => x.DateOfBirth);

        RuleFor(x => x.PeselNumber)
            .NotNull()
            .NotEmpty()
            .Length(11)
            .Must(BeAllDigits)
            .Must(BeValidDate)
            .Must(BeValidChecksum)
                .WithDisplayName(x => x.PeselNumber)
                    .When(x => x.HasPeselNumber);

        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .NotEmpty()
                .WithDisplayName(x => x.PhoneNumber);

        RuleFor(x => x.Roles)
            .NotNull()
                .WithMessage("Property {PropertyName} is required")
            .NotEmpty()
                .WithMessage("Property {PropertyName} cannot be empty")
            .Must(roles => roles.All(role => !string.IsNullOrWhiteSpace(role)))
                .WithMessage("Elements of property {PropertyName} cannot be empty")
            .Must(x => x != null && x.Count > 0)
                .WithMessage("Musisz wybrać przynajmniej jedną Role")
                    .WithDisplayName(x => x.Roles);
    }

    private static bool BeAllDigits(string pesel) => pesel.All(char.IsDigit);

    private static bool BeValidDate(string pesel)
    {
        if (pesel.Length != 11)
        {
            return false;
        }

        // Extract date parts from PESEL
        var year = int.Parse(pesel[..2]);
        var month = int.Parse(pesel.Substring(2, 2));
        var day = int.Parse(pesel.Substring(4, 2));

        // Adjust year and month based on century (PESEL encodes century in the month field)
        (month, year) = month switch
        {
            > 80 => (year += 1800, month -= 80),
            > 60 => (year += 2200, month -= 60),
            > 40 => (year += 2100, month -= 40),
            > 20 => (year += 2000, month -= 20),
            _ => (year += 1900, month)
        };

        return DateTime.TryParse($"{year}-{month:D2}-{day:D2}", out _);
    }

    private static bool BeValidChecksum(string pesel)
    {
        if (pesel.Length != 11)
        { 
            return false; 
        }

        int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        int checksum = 0;

        for (int i = 0; i < 10; i++)
        {
            checksum += weights[i] * (pesel[i] - '0');
        }

        int controlDigit = (10 - (checksum % 10)) % 10;

        return controlDigit == (pesel[10] - '0');
    }
}
