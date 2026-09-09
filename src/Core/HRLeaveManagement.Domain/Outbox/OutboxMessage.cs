using System.Text.Json;

namespace HRLeaveManagement.Domain.Outbox;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; }
    public DateTime OccurredOn { get; private set; }

    public bool IsProcessed { get; private set; }
    public DateTime? ProcessedOn { get; private set; }

    public int RetryCount { get; private set; }
    public string LastError { get; private set; }
    public DateTime? LastErrorOccurredOn { get; private set; }

    private OutboxMessage() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    public static IEnumerable<OutboxMessage> Create(params object[] events)
    {
        var outboxMessages = events.Select(@event => new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = @event.GetType().AssemblyQualifiedName!,
            Payload = JsonSerializer.Serialize(@event),
            OccurredOn = DateTime.UtcNow,
        });

        return outboxMessages;
    }

    /// <summary>
    /// 
    /// </summary>
    public void MarkAsProcessed()
    {
        IsProcessed = true;
        ProcessedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="error"></param>
    public void ErrorOccurred(string error)
    {
        IsProcessed = false;
        LastError = error;
        RetryCount = RetryCount + 1;
        LastErrorOccurredOn = DateTime.UtcNow;
    }
}
