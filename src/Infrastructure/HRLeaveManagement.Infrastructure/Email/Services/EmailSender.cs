using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.DTOs.Email;
using HRLeaveManagement.Infrastructure.Email.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HRLeaveManagement.Infrastructure.Email.Services;

public sealed class EmailSender(IOptions<EmailOptions> emailOptions) : IEmailSender
{
    private readonly EmailOptions _emailOptions = emailOptions.Value;

    public async Task<EmailResponse> SendEmailAsync(EmailMessage email)
    {
        var client = new SendGridClient(_emailOptions.ApiKey);

        var to = new EmailAddress(email.To);
        var from = new EmailAddress(_emailOptions.FromAddress, _emailOptions.FromName);

        var message = MailHelper.CreateSingleTemplateEmail(
            from,
            to,
            email.TemplateId,
            email.TemplatePlaceholders
        );

        var response = await client.SendEmailAsync(message);

        var emailResponse = new EmailResponse(
            IsSuccess: response.IsSuccessStatusCode,
            StatusCode: response.StatusCode,
            ErrorMessage: response.IsSuccessStatusCode 
                ? null
                : await response.Body.ReadAsStringAsync()
        );

        return emailResponse;
    }
}
