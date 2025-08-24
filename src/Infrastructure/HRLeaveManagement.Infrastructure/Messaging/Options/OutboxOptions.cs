namespace HRLeaveManagement.Infrastructure.Messaging.Options;

public class OutboxOptions
{
    public required MessageProcessor MessageProcessor { get; set; }
}

public class MessageProcessor
{
    public required int MessageCollectionPeriodInSeconds { get; set; }
    public required int MessageCountToProcessing { get; set; }
}