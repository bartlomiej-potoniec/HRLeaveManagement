using HRLeaveManagement.Domain.CreationTokens;

namespace HRLeaveManagement.Domain.Entities;

public class Address : Entity
{
    public int Id { get; private set; }

    public string ResidentialAddress { get; private set; }
    public string? SecondaryResidentialAddress { get; private set; }
    public string RegisteredAddress { get; private set; }
    public string? RemoteWorkAddress { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    #region Domain_Navigation_Properties

    public Employee Employee { get; private set; }

    #endregion

    internal Address(Employee employee,
                     string residentialAddress,
                     string registeredAddress,
                     string? secondaryResidentialAddress = null,
                     string? remoteWorkAddress = null,
                     IAddressCreationToken creationToken = default) 
    {
        if (creationToken is null)
        {
            throw new AccessViolationException("Attempted to create Address without proper domain context");
        }

        Employee = employee;
        ResidentialAddress = residentialAddress;
        SecondaryResidentialAddress = secondaryResidentialAddress;
        RegisteredAddress = registeredAddress;
        RemoteWorkAddress = remoteWorkAddress;
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }

    #region Domain_Factory_Methods

    /// <summary>
    /// Updates existing <see cref="Address"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="residentialAddress">Employee main address of residence</param>
    /// <param name="secondaryResidentialAddress">Optional. Employee other address of residence</param>
    /// <param name="registeredAddress">Employee registered residence</param>
    /// <param name="remoteWorkAddress">Employee remote work address</param>
    internal void Update(string residentialAddress,
                       string registeredAddress,
                       string? secondaryResidentialAddress = null,
                       string? remoteWorkAddress = null)
    {
        ResidentialAddress = residentialAddress;
        SecondaryResidentialAddress = secondaryResidentialAddress;
        RegisteredAddress = registeredAddress;
        RemoteWorkAddress = remoteWorkAddress;
        ModifiedAt = DateTime.UtcNow;
    }

    #endregion
}
