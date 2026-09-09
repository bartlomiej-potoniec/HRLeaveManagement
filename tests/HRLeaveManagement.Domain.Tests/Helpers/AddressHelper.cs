using HRLeaveManagement.Domain.Employee.Address;

namespace HRLeaveManagement.Domain.Tests.Helpers;

public class AddressHelper
{
    public static Address CreateAddress(string residentialAddress1 = "Residential_Address_1",
                                        string registeredAddress = "Registered_Address",
                                        string? residentialAddress2 = null,
                                        string? remoteWorkAddress = null)
        => Address.Create(residentialAddress1, registeredAddress, residentialAddress2, remoteWorkAddress);
}
