using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;

namespace HRLeaveManagement.Infrastructure.Messaging;

public class OutboxMetadataProvider(IUserService userService) : IOutboxMetadataProvider
{
    private readonly IUserService _userService = userService;

    public object? GetMetadata()
    {
        var userId = _userService.UserId;
        var employeeId = _userService.EmployeeId;

        return new
        {
            UserId = userId,
            EmployeeId = employeeId
        };
    }
}
