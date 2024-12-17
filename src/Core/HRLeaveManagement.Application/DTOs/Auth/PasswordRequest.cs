namespace HRLeaveManagement.Application.DTOs.Auth;

public record PasswordRequest(string CurrentPassword, string NewPassword);
