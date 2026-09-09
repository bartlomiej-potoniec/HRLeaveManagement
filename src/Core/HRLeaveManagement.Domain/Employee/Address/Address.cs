namespace HRLeaveManagement.Domain.Employee.Address;

public class Address
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

    private Address() {}

    #region Domain_Factory_Methods

    public static Address Create(Employee employee,
                                 string residentialAddress,
                                 string registeredAddress,
                                 string? secondaryResidentialAddress = null,
                                 string? remoteWorkAddress = null)
    {
        Address address = new()
        {
            Employee = employee,
            ResidentialAddress = residentialAddress,
            SecondaryResidentialAddress = secondaryResidentialAddress,
            RegisteredAddress = registeredAddress,
            RemoteWorkAddress = remoteWorkAddress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        return address;
    }

    /// <summary>
    /// Updates existing <see cref="Address"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="residentialAddress">Employee main address of residence</param>
    /// <param name="secondaryResidentialAddress">Optional. Employee other address of residence</param>
    /// <param name="registeredAddress">Employee registered residence</param>
    /// <param name="remoteWorkAddress">Employee remote work address</param>
    public void Update(string residentialAddress,
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
