namespace HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;

public interface IOutboxMetadataProvider
{
    object? GetMetadata();
}
