using HRLeaveManagement.Application.DTOs.Auth;

namespace HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;

public interface IAuthMetadataProvider
{
    AuthMetadata GetMetadata();
}
