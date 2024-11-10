using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.DTOs.Email;
using HRLeaveManagement.Infrastructure.Email.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HRLeaveManagement.Infrastructure.Email.Services;

public sealed class EmailSender(IOptions<EmailOptions> emailSettings) : IEmailSender
{
    private readonly EmailOptions _emailSettings = emailSettings.Value;

    public async Task<bool> SendEmailAsync(EmailMessage email)
    {
        var client = new SendGridClient(_emailSettings.ApiKey);

        var to = new EmailAddress(email.To);
        var from = new EmailAddress(_emailSettings.FromAddress, _emailSettings.FromName);

        var message = MailHelper.CreateSingleEmail(
            from: from,
            to: to,
            subject: email.Subject,
            plainTextContent: email.TextContent,
            htmlContent: email.TextContent
        );

        var response = await client.SendEmailAsync(message);
        return response.IsSuccessStatusCode;
    }
}
