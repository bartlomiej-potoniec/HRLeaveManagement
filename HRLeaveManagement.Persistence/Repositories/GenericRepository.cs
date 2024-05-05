using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Common;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public abstract class GenericRepository<T>(ApplicationDbContext context) 
    : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context = context;

    public async Task<IReadOnlyList<T>> GetAllAsync() => await _context
        .Set<T>()
        .AsNoTracking()
        .ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await _context
        .Set<T>()
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<int> CreateAsync(T entity)
    {
        await _context.AddAsync(entity);
        _context.Entry(entity).State = EntityState.Added;

        await SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Update(entity);
        _context.Entry(entity).State = EntityState.Modified;

        await SaveChangesAsync();
    }
    public async Task DeleteAsync(T entity)
    {
        _context.Remove(entity);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
