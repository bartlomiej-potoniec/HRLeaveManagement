namespace HRLeaveManagement.Application.DTOs.Auth;

public record AuthResponse(string Id,
                           string UserName,
                           string Email,
                           string Token);