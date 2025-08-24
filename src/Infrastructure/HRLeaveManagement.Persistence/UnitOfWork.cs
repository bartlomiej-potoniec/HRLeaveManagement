using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Persistence.DbContexts;

namespace HRLeaveManagement.Persistence;

public sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
