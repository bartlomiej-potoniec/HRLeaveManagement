using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;
using HRLeaveManagement.Application.DTOs.Auth;

namespace HRLeaveManagement.Infrastructure.Messaging;

public class AuthMetadataProvider(IUserService userService) : IAuthMetadataProvider
{
    private readonly IUserService _userService = userService;

    public AuthMetadata GetMetadata()
    {
        var requestingUserId = _userService.UserId;
        var requestingEmployeeId = _userService.EmployeeId;
        var requestingUserName = _userService.UserName;
        var requestingUserEmail = _userService.UserEmail;

        return new()
        {
            RequestingUserId = requestingUserId,
            RequestingEmployeeId = requestingEmployeeId,
            RequestingUserName = requestingUserName,
            RequestingUserEmail = requestingUserEmail
        };
    }
}
