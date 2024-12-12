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
                    var randomNumber = random.Next(1, 9);
                    passwordBuilder.Append(randomNumber);

                    break;
                case 1:
                    var randomSmallLetter = (char)random.Next(98, 123);
                    passwordBuilder.Append(randomSmallLetter);

                    break;
                case 2:
                    var randomCapitalLetter = (char)random.Next(66, 91);
                    passwordBuilder.Append(randomCapitalLetter);

                    break;
                default:
                    var randomIndexer = random.Next(_credentialOptions.Password.AllowedSpecialChars.Length);
                    var randomSpecialChar = _credentialOptions.Password.AllowedSpecialChars[randomIndexer];
                    passwordBuilder.Append(randomSpecialChar);

                    break;
            }
        }

        return passwordBuilder.ToString();
    }
}
