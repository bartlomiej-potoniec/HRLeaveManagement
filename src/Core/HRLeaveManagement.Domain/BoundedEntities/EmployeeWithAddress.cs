using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.BoundedEntities;

public class EmployeeWithAddress(Employee employee)
{
    private Employee Employee => employee;
    public Address Address => Employee.Address;

    public void Update(string position,
                       string responsibilities,
                       string residentialAddress,
                       string registeredAddress,
                       string? secondaryResidentialAddress = null,
                       string? remoteWorkAddress = null,
                       Section? section = null,
                       Employee? leader = null)
        => Employee.Update(
            position,
            responsibilities,
            residentialAddress,
            registeredAddress,
            secondaryResidentialAddress,
            remoteWorkAddress,
            section,
            leader
        );
}
