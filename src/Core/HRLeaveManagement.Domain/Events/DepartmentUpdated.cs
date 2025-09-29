using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class DepartmentUpdated(Department department) : IEntityEvent
{
    public int DepartmentId => department.Id;
    public string DepartmentName => department.Name;

    public DateTime OccurredOn => DateTime.UtcNow;

    public string Content => $"Zauktualizowano dział nr: { DepartmentId } o (nowej) nazwie: { DepartmentName }";

}
