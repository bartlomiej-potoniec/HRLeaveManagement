namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IEmailService
{
    Task SendRegistrationEmailAsync(string email,
                               string firstName,
                               string userName,
                               string password,
                               string confirmationLink,
                               CancellationToken cancellationToken);
    Task SendEmployeeCreationEmailAsync(string email, string firstname, CancellationToken cancellationToken);
    string GenerateEmailConfirmationLinkAsync(string userId, string token, CancellationToken cancellationToken);
}
