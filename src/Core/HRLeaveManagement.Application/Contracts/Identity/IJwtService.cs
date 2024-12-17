namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IJwtService
{
    Task<string> GenerateJwtToken(string userName);
}
