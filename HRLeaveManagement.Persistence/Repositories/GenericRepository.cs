using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Common;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) 
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
        _context.Entry(entity).State = EntityState.Added;
        await _context.AddAsync(entity);

        await SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.Update(entity);

        await SaveChangesAsync();
    }
    public async Task DeleteAsync(T entity)
    {
        _context.Remove(entity);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
