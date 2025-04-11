using HRLeaveManagement.Identity.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace HRLeaveManagement.Identity.Tests.Fakes;

public class FakeApplicationIdentityDbContext : ApplicationIdentityDbContext
{
    private readonly Mock<DbContext> _dbContextMock;
    private readonly Mock<DatabaseFacade> _databaseMock;

    public override DatabaseFacade Database => _databaseMock.Object;

    public FakeApplicationIdentityDbContext()
        : base(new DbContextOptionsBuilder<ApplicationIdentityDbContext>().Options)
    {
        _dbContextMock = new();
        _databaseMock = new(_dbContextMock.Object);
    }

    public void SetupToThrowOperationCanceledException()
        => _databaseMock
            .Setup(db => db.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException("Simulated cancellation"));

    public void SetupSetupToReturnDbContextTransaction(IDbContextTransaction transaction)
        => _databaseMock
            .Setup(db => db.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);
}
