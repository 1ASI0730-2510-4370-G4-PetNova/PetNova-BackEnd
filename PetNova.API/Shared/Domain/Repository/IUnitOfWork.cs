namespace PetNova.API.Shared.Domain.Repository;

public interface IUnitOfWork : IDisposable
{
    Task CompleteAsync();
}
