namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IEmailService
{
    Task SendRegistrationEmail(string email,
                               string firstName,
                               string userName,
                               string password,
                               string confirmationLink);
    Task SendEmployeeCreationEmail(string email, string firstname);
    string GenerateEmailConfirmationLink(string userId, string token);
}
