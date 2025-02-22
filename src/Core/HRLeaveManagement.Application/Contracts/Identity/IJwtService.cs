namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IJwtService
{
    Task<string> GenerateJwtTokenAsync(string userName, CancellationToken cancellationToken);
}
