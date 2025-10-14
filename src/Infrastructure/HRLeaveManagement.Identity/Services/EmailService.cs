using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.DTOs.Email;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using HRLeaveManagement.Infrastructure.Email.Options;

namespace HRLeaveManagement.Identity.Services;

public sealed class EmailService(IEmailSender emailSender,
                                 IHttpContextAccessor httpContextAccessor,
                                 IUrlHelperFactory urlHelperFactory,
                                 IOptions<EmailOptions> emailOptions,
                                 IAppLogger<EmailService> logger) 
    : IEmailService
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IUrlHelperFactory _urlHelperFactory = urlHelperFactory;
    private readonly IAppLogger<EmailService> _logger = logger;
    private readonly EmailOptions _emailOptions = emailOptions.Value;

    public async Task SendRegistrationEmailAsync(string email,
                                                 string firstName,
                                                 string userName,
                                                 string password,
                                                 string confirmationLink,
                                                 CancellationToken cancellationToken = default)
    {
        var emailMessage = new EmailMessage
        {
            To = email,
            Subject = "An account in HrManagementSystem was created for you!",
            TemplateId = _emailOptions.TemplatesId["Registration"],
            TemplatePlaceholders = new
            {
                FirstName = firstName,
                UserName = userName,
                Password = password,
                ConfirmationLink = confirmationLink
            }
        };

        _logger.LogInformation("Sending registration email to {Email}", email);

        var emailResult = await _emailSender.SendEmailAsync(emailMessage, cancellationToken);

        if (!emailResult.IsSuccess)
        {
            _logger.LogError("Sending email failed to {Email}", email);
            throw new InvalidOperationException(emailResult.ErrorMessage);
        }

        _logger.LogInformation("Sending email successful to {Email}", email);
    }

    public async Task SendEmployeeCreationEmailAsync(string email,
                                                     string firstname,
                                                     CancellationToken cancellationToken = default)
    {
        var emailMessage = new EmailMessage
        {
            To = email,
            Subject = "An Employee informations in HrManagementSystem was completed for your account!",
            TemplateId = _emailOptions.TemplatesId["EmployeeCreation"],
            TemplatePlaceholders = new
            {
                FirstName = firstname,
                ManualDownloadLink = "" /* Manual PDF from FTP server in feature */
            }
        };

        _logger.LogInformation("Sending Employee-Creation email to {Email}", email);

        var emailResult = await _emailSender.SendEmailAsync(emailMessage, cancellationToken);

        if (!emailResult.IsSuccess)
        {
            _logger.LogError("Sending email failed to {Email}", email);
            throw new InvalidOperationException(emailResult.ErrorMessage);
        }

        _logger.LogInformation("Sending email successful to {Email}", email);
    }

    public string GenerateEmailConfirmationLink(string userId,
                                                string token,
                                                CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext is not available");

        var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext
        {
            HttpContext = httpContext,
            RouteData = httpContext.GetRouteData(),
            ActionDescriptor = new ActionDescriptor()
        });

        var actionLink = urlHelper
            .ActionLink(
                action: "ConfirmEmail",
                controller: "Auth",
                values: new { userId, token },
                protocol: httpContext.Request.Scheme
            )
            ?? throw new InvalidOperationException("An error occurred while creating confirmation link");

        return actionLink;
    }

    public async Task SendDepartmentCreationEmailAsync(string email,
                                                       int departmentId,
                                                       string departmentName,
                                                       CancellationToken cancellationToken)
    {
        await Task.CompletedTask;   
    }

    public async Task SendEmployeeContractCreationEmailAsync(string email,
                                                             string contractType,
                                                             string employeeName,
                                                             CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public async Task SendEmployeeUpdatingEmailAsync(string email,
                                                     string employeeId,
                                                     string firstName,
                                                     string lastName,
                                                     CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public Task SendEmployeeUpdatingEmailAsync(string email, Guid employeeId, string firstName, string lastName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendLeaveAllocationUdpatingEmailAsync(string requestingUserEmail, Guid employeeId, string employeeFirstName, string employeeLastName, int? availableDays, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendLeaveRequestCreationEmail(string requestingUserEmail, string requesterFullName, string approverFullName, string leaveTypeName, DateOnly leaveStartedAt, DateOnly leaveEndedAt, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendLeaveRequestCancelationEmail(string requestingUserEmail, string requestingUserName, string leaveTypeName, DateTime leaveRequestCreatedAt, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendLeaveRequestApprovalEmailAsync(string requestingUserEmail, string requestingUserName, string leaveTypeName, DateTime leaveRequestCreatedAt, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendLeaveRequestRejectionEmailAsync(string requestingUserEmail, string requestingUserName, string leaveTypeName, DateTime leaveRequestCreatedAt, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
