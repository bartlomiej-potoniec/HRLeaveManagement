namespace HRLeaveManagement.Application.Contracts.Infrastructure.Logging;

public interface IAppLogger<T>
{
    void LogDebug(string message, params object[] args);
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(string message, params object[] args);
}
