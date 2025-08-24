using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.BoundedEntities;

public class EmployeeWithRemoteWorkLimits(Employee employee)
{
    internal Employee Employee => employee;
    public IReadOnlyList<RemoteWorkLimit> RemoteWorkLimits => Employee.RemoteWorkLimits;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="remoteWorkLimit"></param>
    public void AddRemoteWorkLimit(RemoteWorkLimit remoteWorkLimit) => Employee.AddRemoteWorkLimit(remoteWorkLimit);
}
