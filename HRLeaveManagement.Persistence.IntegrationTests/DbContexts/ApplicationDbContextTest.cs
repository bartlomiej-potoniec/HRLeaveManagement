using FluentAssertions;
using HRLeaveManagement.Domain;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.IntegrationTests.DbContexts;

public class ApplicationDbContextTest
{
    private readonly ApplicationDbContext _dbContext;

    public ApplicationDbContextTest()
    {
        var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("test_database")
            .Options;

        _dbContext = new ApplicationDbContext(dbOptions);
    }

    [Fact]
    public async Task Save_SetDateCreatedValue()
    {
        // Arrange
        var leaveType = new LeaveType { Id = 10, DefaultDays = 10, Name = "Test Vacation" };

        // Act
        await _dbContext.LeaveTypes.AddAsync(leaveType);
        await _dbContext.SaveChangesAsync();

        // Assert
        leaveType.CreatedAt
            .Should()
            .NotBeNull();
    }
    
    [Fact]
    public async Task Save_SetDateModifiedValue()
    {
        // Arrange
        var dateTime = DateTime.Now;
        var leaveType = new LeaveType { Id = 11, DefaultDays = 10, Name = "Test Vacation" };

        // Act
        await _dbContext.LeaveTypes.AddAsync(leaveType);
        await _dbContext.SaveChangesAsync();

        // Assert
        leaveType.ModifiedAt
            .Should()
            .NotBeNull();

        leaveType.ModifiedAt
            .Should()
            .BeOnOrAfter(dateTime);
    }
}
