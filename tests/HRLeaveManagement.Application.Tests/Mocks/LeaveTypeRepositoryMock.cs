using HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Contracts.Persistence;
using Moq;

namespace HRLeaveManagement.Application.Tests.Mocks;

public class LeaveTypeRepositoryMock
{
    public static Mock<ILeaveTypeRepository> GetLeaveTypeLeaveTypeRepositoryMock()
    {
        List<LeaveType> leaveTypes = [
            new() { Id = 1, DefaultDays = 10, Name = "Test Vacation" },
            new() { Id = 2, DefaultDays = 15, Name = "Test Sick" },
            new() { Id = 3, DefaultDays = 15, Name = "Test Maternity" },
        ];

        var mock = new Mock<ILeaveTypeRepository>();
        
        mock
            .Setup(mock => mock.GetAllAsync())
            .ReturnsAsync(leaveTypes);

        mock
            .Setup(mock => mock.CreateAsync(It.IsAny<LeaveType>()))
            .ReturnsAsync((LeaveType leaveType) =>
            {
                leaveTypes.Add(leaveType);
                return leaveType.Id;
            });

        return mock;
    }
}
