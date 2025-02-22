using HRLeaveManagement.Application.DTOs.Email;

namespace HRLeaveManagement.Application.Contracts.Infrastructure.Email;

public interface IEmailSender
{
    Task<EmailResponse> SendEmailAsync(EmailMessage email, CancellationToken cancellationToken);
}
