namespace PetNova.API.Shared.Domain.Repository;

public interface IRepository<TEntity, in TId> where TEntity : class
{
    Task<TEntity?> FindByIdAsync(TId id);
    Task<IEnumerable<TEntity>> ListAsync();
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<bool> Exists(TId id);
}
