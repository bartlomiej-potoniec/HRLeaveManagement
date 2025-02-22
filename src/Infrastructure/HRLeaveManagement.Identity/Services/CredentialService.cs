using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.RegularExpressions;

namespace HRLeaveManagement.Identity.Services;

public sealed class CredentialService(UserManager<ApplicationUser> userManager,
                                      IOptions<CredentialOptions> credentialOptions) 
    : ICredentialService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly CredentialOptions _credentialOptions = credentialOptions.Value;

    /// <summary>
    /// Generates a unique username based on the user's first name, last name, and date of birth.
    /// </summary>
    /// <param name="firstname">The user's first name.</param>
    /// <param name="lastname">The user's last name.</param>
    /// <param name="dateOfBirth">The user's date of birth in a string format.</param>
    /// <returns>
    /// A unique username format
    /// </returns>
    public string GenerateUserLogin(string firstname, string lastname, string dateOfBirth)
    {
        var users = _userManager.Users.ToList();

        var input = $"{ firstname } { lastname } { dateOfBirth }";
        var pattern = @"^(?<firstLetter>\w)\w*\s(?<lastSix>[a-zA-Z-]{1,6})\S*\s\d{2}(?<dob>\d{2})";

        var match = Regex.Match(input, pattern);

        var login = $"{ match.Groups["firstLetter"].Value }{ match.Groups["lastSix"].Value }{ match.Groups["dob"].Value }"
            .Trim()
            .Replace("-", "")
            .ToLower();

        while (users.Find(u => u.UserName == login) is not null)
        {
            var randomValue = new Random()
                .Next(
                    _credentialOptions.Login.MinRandomValue, 
                    _credentialOptions.Login.MaxRandomValue)
                .ToString("D2");

            login = login.Replace(match.Groups["dob"].Value, randomValue);
        }
        
        return login;
    }

    /// <summary> 
    /// Generates a random password based on the configured password options.
    /// </summary>
    /// <returns>
    /// A randomly generated password string that includes numbers, lowercase letters, 
    /// uppercase letters, and special characters according to the defined rules.
    /// </returns>
    public string GenerateUserPassword()
    {
        Random random = new();
        StringBuilder passwordBuilder = new();

        for (int i = 0; i < _credentialOptions.Password.Length; i++)
        {
            var randomValue = random.Next(4);

            switch (randomValue)
            {
                case 0:
                    // Generates a random digit (1-9)
                    var randomNumber = random.Next(1, 10);
                    passwordBuilder.Append(randomNumber);

                    break;
                case 1:
                    // Generates a random lowercase letter (a-z)
                    // ASCII codes: 'a' = 97, 'z' = 122
                    var randomSmallLetter = (char)random.Next(97, 123);
                    passwordBuilder.Append(randomSmallLetter);

                    break;
                case 2:
                    // Generates a random UPPERCASE letter (A-Z)
                    // ASCII codes: 'A' = 65, 'Z' = 90
                    var randomCapitalLetter = (char)random.Next(65, 91);
                    passwordBuilder.Append(randomCapitalLetter);

                    break;
                case 3:
                default:
                    // Select a random special character from the allowed set
                    var randomIndexer = random.Next(_credentialOptions.Password.AllowedSpecialChars.Length);
                    var randomSpecialChar = _credentialOptions.Password.AllowedSpecialChars[randomIndexer];
                    passwordBuilder.Append(randomSpecialChar);

                    break;
            }
        }

        return passwordBuilder.ToString();
    }
}
