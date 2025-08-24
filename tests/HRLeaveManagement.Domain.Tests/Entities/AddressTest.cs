namespace HRLeaveManagement.Domain.Tests.Entities;

public class AddressTest
{
    [Fact]
    public void EmployeeCreate_ForGivenAddressParams_ReturnsNewInstanceWithEmployeeAddress()
    {
        // Arrange
        string position = "Engineer";
        string responsibilities = "Engineering";
        string residentialAddress1 = "Residential address1";
        string registeredAddress = "Registered address";
        string? residentialAddress2 = "Residential address2";
        string? remoteWorkAddress = residentialAddress1;

        var expectedAddress = new
        {
            ResidentialAddress1 = residentialAddress1,
            RegisteredAddress = registeredAddress,
            ResidentialAddress2 = residentialAddress2,
            RemoteWorkAddress = remoteWorkAddress
        };

        // Act
        Employee employee = Employee.Create(
            position,
            responsibilities,
            residentialAddress1: residentialAddress1,
            registeredAddress: registeredAddress,
            residentialAddress2: residentialAddress2,
            remoteWorkAddress: remoteWorkAddress
        );

        // Assert
        employee.Address
            .Should()
            .BeOfType<Address>();

        employee.Address
            .Should()
            .BeEquivalentTo(expectedAddress, options => options
                .Including(a => a.ResidentialAddress1)
                .Including(a => a.RegisteredAddress)
                .Including(a => a.ResidentialAddress2)
                .Including(a => a.RemoteWorkAddress)
            );
    }

    [Fact]
    public void Update_ForGivenParams_UpdatesPropertiesOfExistingInstance()
    {
        // Arrange
        string residentialAddress1 = "Res.Addr.1";
        string registeredAddress = "Reg.Addr";
        string? residentialAddress2 = "Res.Addr.2";
        string? remoteWorkAddress = residentialAddress1;

        var expectedAddress = new
        {
            ResidentialAddress1 = residentialAddress1,
            RegisteredAddress = registeredAddress,
            ResidentialAddress2 = residentialAddress2,
            RemoteWorkAddress = remoteWorkAddress
        };

        Employee employee = Employee.Create(
            "Engineer",
            "Engineering",
            residentialAddress1: "Residential address1",
            registeredAddress: "Registered address",
            residentialAddress2: "Residential address2",
            remoteWorkAddress: "Residential address1"
        );

        Address employeeAddress = employee.Address;

        // Act
        employeeAddress.Update(residentialAddress1, registeredAddress, residentialAddress2, remoteWorkAddress);

        // Assert
        employeeAddress
            .Should()
            .BeEquivalentTo(expectedAddress, options => options
                .Including(a => a.ResidentialAddress1)
                .Including(a => a.RegisteredAddress)
                .Including(a => a.ResidentialAddress2)
                .Including(a => a.RemoteWorkAddress)
            );
    }
}
