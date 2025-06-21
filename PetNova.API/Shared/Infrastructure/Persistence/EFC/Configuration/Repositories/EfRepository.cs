using Microsoft.EntityFrameworkCore;
using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Repositories;

public class EfRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public EfRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> FindByIdAsync(TId id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<TEntity>> ListAsync() => await _dbSet.ToListAsync();
    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
    public void Update(TEntity entity) => _dbSet.Update(entity);
    public void Remove(TEntity entity) => _dbSet.Remove(entity);
    public async Task<bool> Exists(TId id) => await _dbSet.FindAsync(id) != null;
    
}
