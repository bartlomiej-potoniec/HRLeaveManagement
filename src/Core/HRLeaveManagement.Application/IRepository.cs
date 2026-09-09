using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application;

public interface IRepository<TEntity> 
    where TEntity : IRootEntity
{
    Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> WhereAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
    void Add(TEntity entity);
}
