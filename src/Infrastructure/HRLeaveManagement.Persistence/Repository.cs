using HRLeaveManagement.Application;
using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Persistence;

public sealed class Repository<TEntity>(ApplicationDbContext dbContext) : IRepository<TEntity> 
    where TEntity : IRootEntity
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> WhereAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Add(TEntity entity)
    {
        throw new NotImplementedException();
    }
}
