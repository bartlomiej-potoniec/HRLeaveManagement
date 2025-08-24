using HRLeaveManagement.Domain.Contracts;

namespace HRLeaveManagement.Domain.Entities;

public abstract class Entity
{
    private readonly List<IEntityEvent> _events = [];
    public IReadOnlyList<IEntityEvent> Events => _events.AsReadOnly();

    protected Entity() {}

    /// <summary>
    /// Add particular-type event do the event list
    /// </summary>
    /// <param name="event">particular-type event</param>
    internal protected void AddEvent(IEntityEvent @event) => _events.Add(@event);

    /// <summary>
    /// Clears all event list so that no event occure
    /// </summary>
    internal void ClearEvents() => _events.Clear();
}
