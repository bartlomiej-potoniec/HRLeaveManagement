using System.Text.Json;

namespace HRLeaveManagement.Domain.Entities;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public string Publisher { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; }
    public string Content { get; private set; }
    public string? Metadata { get; private set; }
    public DateTime OccurredOn { get; private set; }

    public bool IsProcessed { get; private set; }
    public DateTime? ProcessedOn { get; private set; }

    public int RetryCount { get; private set; }
    public string Error { get; private set; }
    public DateTime? ErrorOccurredOn { get; private set; }

    private OutboxMessage() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public static IReadOnlyList<OutboxMessage> CreateForEntity(Entity entity, object? metadata = null)
    {
        var outboxMessages = entity.Events
            .Select(@event => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = @event.GetType().AssemblyQualifiedName!,
                Payload = JsonSerializer.Serialize(@event),
                Content = @event.Content,
                Metadata = metadata is null
                    ? null
                    : JsonSerializer.Serialize(metadata),
                OccurredOn = @event.OccurredOn,
            })
            .ToList();

        entity.ClearEvents();
        return outboxMessages.AsReadOnly();
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
        Error = error;
        RetryCount++;
        ErrorOccurredOn = DateTime.UtcNow;
    }
}
