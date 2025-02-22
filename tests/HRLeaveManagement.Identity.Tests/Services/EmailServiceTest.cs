using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Email;
using HRLeaveManagement.Identity.Services;
using HRLeaveManagement.Infrastructure.Email.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System.Net;

namespace HRLeaveManagement.Identity.Tests.Services;

public class EmailServiceTest
{
    [Fact]
    public async Task SendRegistrationEmail_ThrowsInvalidOperationException_WhenEmailSendingFailed()
    {
        // Arrange
        var emailSenderMock = CreateEmailSenderMock();
        var emailResponse = new EmailResponse(false, HttpStatusCode.Forbidden, "Cannot send an email");

        string email = "jkowalski95@company.com";
        string firstName = "Jan";
        string userName = "Kowalski";
        string password = "P@ssword1";
        string confirmationLink = "confirmation_link";

        emailSenderMock
            .Setup(es => es.SendEmailAsync(It.IsAny<EmailMessage>()))
            .ReturnsAsync(emailResponse);

        var emailService = CreateEmailService(emailSenderMock);
        var expectedExceptionMessage = "Cannot send an email";

        // Act
        Func<Task> result = () => emailService.SendRegistrationEmailAsync(email, firstName, userName, password, confirmationLink);

        // Assert
        await result
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void GenerateEmailConfirmationLink_ThrowsInvalidOperationException_WhenHttpContextNotAvailable()
    {
        // Arrange
        var userId = "user_id";
        var token = "token";

        var emailService = CreateEmailService();

        var expectedExceptionMessage = "HttpContext is not available";

        // Act
        Action result = () => emailService.GenerateEmailConfirmationLinkAsync(userId, token);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void GenerateEmailConfirmationLink_ThrowsInvalidOperationException_WhenCreatingActionLinkFailed()
    {
        // Arrange
        var userId = "user_id";
        var token = "token";
        var httpContext = CreateDefaultHttpContext();

        var urlHelperMock = CreateUrlHelperMock();
        var httpContextAccessorMock = CreateHttpContextAccessorMock();
        var urlHelperFactoryMock = CreateUrlHelperFactoryMock();

        SetupHttpContextAccessorMockToReturnHttpContext(httpContextAccessorMock, httpContext);
        SetupUrlHelperFactoryMockToReturnUrlHelper(urlHelperFactoryMock, urlHelperMock.Object);
        SetupUrlHelperMockToReturnActionContextWithHttpContext(urlHelperMock, httpContext);

        urlHelperMock
            .Setup(url => url.Action(It.IsAny<UrlActionContext>()))
            .Returns((UrlActionContext context) => null);

        var emailService = CreateEmailService(
            httpContextAccessorMock: httpContextAccessorMock,
            urlHelperFactoryMock: urlHelperFactoryMock
        );

        var expectedExceptionMessage = "An error occurred while creating confirmation link";

        // Act
        Action result = () => emailService.GenerateEmailConfirmationLinkAsync(userId, token);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void GenerateEmailConfirmationLink_ReturnsConfirmationActionLink()
    {
        // Arrange
        var userId = "user_id";
        var token = "token";
        var httpContext = CreateDefaultHttpContext();

        var urlHelperMock = CreateUrlHelperMock();
        var httpContextAccessorMock = CreateHttpContextAccessorMock();
        var urlHelperFactoryMock = CreateUrlHelperFactoryMock();

        var actionLink = "https://scheme/Auth/ConfirmEmail?userId=user_id&token=token";

        SetupHttpContextAccessorMockToReturnHttpContext(httpContextAccessorMock, httpContext);
        SetupUrlHelperFactoryMockToReturnUrlHelper(urlHelperFactoryMock, urlHelperMock.Object);
        SetupUrlHelperMockToReturnActionContextWithHttpContext(urlHelperMock, httpContext);

        urlHelperMock
            .Setup(url => url.Action(It.IsAny<UrlActionContext>()))
            .Returns((UrlActionContext context) => actionLink);

        var emailService = CreateEmailService(
            httpContextAccessorMock: httpContextAccessorMock,
            urlHelperFactoryMock: urlHelperFactoryMock
        );

        // Act
        var result = emailService.GenerateEmailConfirmationLinkAsync(userId, token);

        // Assert
        result
            .Should()
            .Be(actionLink);
    }

    #region Test_Factory_Methods

    private static DefaultHttpContext CreateDefaultHttpContext() => new();
    private static Mock<IUrlHelper> CreateUrlHelperMock() => new();
    private static Mock<IEmailSender> CreateEmailSenderMock() => new();
    private static Mock<IHttpContextAccessor> CreateHttpContextAccessorMock() => new();
    private static Mock<IUrlHelperFactory> CreateUrlHelperFactoryMock() => new();

    private static OptionsWrapper<EmailOptions> SetupOptionsToReturnEmailOptions(EmailOptions options) => new(options);
    
    private static void SetupHttpContextAccessorMockToReturnHttpContext(Mock<IHttpContextAccessor> httpContextAccessorMock,
                                                                        HttpContext httpContext)
        => httpContextAccessorMock
            .Setup(http => http.HttpContext)
            .Returns(httpContext);

    private static void SetupUrlHelperFactoryMockToReturnUrlHelper(Mock<IUrlHelperFactory> urlHelperFactoryMock,
                                                                   IUrlHelper urlHelper)
        => urlHelperFactoryMock
            .Setup(url => url.GetUrlHelper(It.IsAny<ActionContext>()))
            .Returns(urlHelper);

    private static void SetupUrlHelperMockToReturnActionContextWithHttpContext(Mock<IUrlHelper> urlHelperMock,
                                                                               HttpContext httpContext)
        => urlHelperMock
            .Setup(url => url.ActionContext)
            .Returns(new ActionContext { HttpContext = httpContext });

    private static EmailService CreateEmailService(Mock<IEmailSender>? emailSenderMock = null,
                                                   Mock<IHttpContextAccessor>? httpContextAccessorMock = null,
                                                   Mock<IUrlHelperFactory>? urlHelperFactoryMock = null)
    {
        var loggerMock = new Mock<IAppLogger<EmailService>>();

        emailSenderMock ??= new Mock<IEmailSender>();
        httpContextAccessorMock ??= new Mock<IHttpContextAccessor>();
        urlHelperFactoryMock ??= new Mock<IUrlHelperFactory>();

        var emailOptions = CreateEmailOptions();
        var options = SetupOptionsToReturnEmailOptions(emailOptions);

        return new(
            emailSenderMock.Object,
            httpContextAccessorMock.Object,
            urlHelperFactoryMock.Object,
            options,
            loggerMock.Object
        );
    }

    private static EmailOptions CreateEmailOptions()
        => new()
        {
            ApiKey = "api_key",
            FromAddress = "my_address@hr.com",
            FromName = "HR Management Test",
            TemplatesId = new Dictionary<string, string>
            {
                { "Registration", "test-template-id" }
            }
        };

    #endregion
}
