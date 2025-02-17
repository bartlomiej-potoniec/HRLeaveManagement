namespace HRLeaveManagement.Application.DTOs.Users;

public record LockoutUserAccountRequest(Guid UserId, DateTime LockoutEnd);
