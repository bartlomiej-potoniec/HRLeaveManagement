using System.Net;
using HRLeaveManagement.Api.Models;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Exceptions;

namespace HRLeaveManagement.Api.Middleware;

public sealed class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext, IAppLogger<GlobalExceptionHandlerMiddleware> logger)
    {
        try
        {
            await _next.Invoke(httpContext);
        }

        catch (Exception ex)
        {
            logger.LogError("An exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        CustomProblemDetails problemDetails = new();

        switch (ex)
        {
            case BadRequestException badRequestException:
                statusCode = HttpStatusCode.BadRequest;
                problemDetails = new CustomProblemDetails
                {
                    Title = badRequestException.Message,
                    Status = (int)statusCode,
                    Type = nameof(BadRequestException),
                    Detail = badRequestException.InnerException?.Message,
                    Errors = badRequestException.ValidationErrors
                };
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                problemDetails = new CustomProblemDetails
                {
                    Title = notFoundException.Message,
                    Status = (int)statusCode,
                    Type = nameof(NotFoundException),
                    Detail = notFoundException.InnerException?.Message,
                };
                break;
            default:
                problemDetails = new CustomProblemDetails
                {
                    Title = ex.Message,
                    Status = (int)statusCode,
                    Type = nameof(HttpStatusCode.InternalServerError),
                    Detail = ex.StackTrace,
                };
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }
}
