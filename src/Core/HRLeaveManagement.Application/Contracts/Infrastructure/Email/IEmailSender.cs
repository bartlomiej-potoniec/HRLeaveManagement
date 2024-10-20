using HRLeaveManagement.Application.Models.Email;

namespace HRLeaveManagement.Application.Contracts.Infrastructure.Email;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(EmailMessage email);
}
