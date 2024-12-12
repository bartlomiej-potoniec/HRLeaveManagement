namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IEmailService
{
    Task SendRegistrationEmail(string email, string firstName, string userName, string password, string confirmationLink);
    string GenerateEmailConfirmationLink(string userId, string token);
}
