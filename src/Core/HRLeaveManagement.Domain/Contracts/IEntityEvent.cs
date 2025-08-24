namespace HRLeaveManagement.Domain.Contracts;

public interface IEntityEvent
{
    string Content { get; }
    DateTime OccurredOn { get; }
}
