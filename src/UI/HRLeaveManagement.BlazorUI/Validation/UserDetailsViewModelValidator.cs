using HRLeaveManagement.BlazorUI.ViewModels.Users;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class UserDetailsViewModelValidator : AbstractValidator<UserDetailsViewModel>
{
    public UserDetailsViewModelValidator()
    {
        RuleFor(vm => vm.FirstName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(vm => vm.LastName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(vm => vm.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(vm => vm.DateOfBirth)
            .NotNull()
            .NotEmpty()
            .GreaterThan(new DateTime((DateTime.Now.Year - 100), 1, 1))
            .LessThan(new DateTime((DateTime.Now.Year - 18), 1, 1));

        RuleFor(vm => vm.PeselNumber)
            .Length(11)
            .Must(BeAllDigits)
            .Must(BeValidDate)
            .Must(BeValidChecksum);

        RuleFor(vm => vm.PhoneNumber)
            .NotNull()
            .NotEmpty();

        RuleFor(vm => vm.Roles)
            .NotNull()
                .WithMessage("Property {PropertyName} is required")
            .NotEmpty()
                .WithMessage("Property {PropertyName} cannot be empty")
            .Must(roles => roles.All(role => !string.IsNullOrWhiteSpace(role)))
                .WithMessage("Elements of property {PropertyName} cannot be empty")
            .Must(x => x != null && x.Count > 0)
                .WithMessage("Musisz wybrać przynajmniej jedną Role");
    }

    private static bool BeAllDigits(string pesel) => pesel.All(char.IsDigit);

    private static bool BeValidDate(string pesel)
    {
        if (pesel.Length != 11) return false;

        // Extract date parts from PESEL
        var year = int.Parse(pesel.Substring(0, 2));
        var month = int.Parse(pesel.Substring(2, 2));
        var day = int.Parse(pesel.Substring(4, 2));

        // Adjust year and month based on century (PESEL encodes century in the month field)
        if (month > 80) { year += 1800; month -= 80; }
        else if (month > 60) { year += 2200; month -= 60; }
        else if (month > 40) { year += 2100; month -= 40; }
        else if (month > 20) { year += 2000; month -= 20; }
        else { year += 1900; }

        return DateTime.TryParse($"{year}-{month:D2}-{day:D2}", out _);
    }

    private static bool BeValidChecksum(string pesel)
    {
        if (pesel.Length != 11) return false;

        int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        int checksum = 0;

        for (int i = 0; i < 10; i++)
            checksum += weights[i] * (pesel[i] - '0');

        int controlDigit = (10 - (checksum % 10)) % 10;

        return controlDigit == (pesel[10] - '0');
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<UserDetailsViewModel>.CreateWithOptions((UserDetailsViewModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
