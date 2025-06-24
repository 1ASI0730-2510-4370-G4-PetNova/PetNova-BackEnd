using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Status.Interface.DTOs;

using StatusEntity = PetNova.API.Veterinary.Status.Domain.Model.Aggregate.Status;

namespace PetNova.API.Veterinary.Status.Application.Services;

public sealed class StatusService : IStatusService
{
    private readonly IRepository<StatusEntity, Guid> _repo;
    private readonly IUnitOfWork                     _uow;

    public StatusService(IRepository<StatusEntity, Guid> repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    /* ---------- mapping ---------- */
    private static StatusDTO Map(StatusEntity s) => new()
    {
        Id          = s.Id,
        Name        = s.Name,
        Description = s.Description,
        Type        = s.Type,
        IsActive    = s.IsActive
    };

    /* ---------- CRUD ---------- */

    public async Task<IEnumerable<StatusDTO>> ListAsync() =>
        (await _repo.ListAsync()).Select(Map);

    public async Task<IEnumerable<StatusDTO>> ListByTypeAsync(string type) =>
        (await _repo.ListAsync())
        .Where(s => s.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
        .Select(Map);

    public async Task<StatusDTO?> GetByIdAsync(Guid id) =>
        (await _repo.FindByIdAsync(id)) is { } s ? Map(s) : null;

    public async Task<StatusDTO> CreateAsync(StatusDTO dto)
    {
        var s = new StatusEntity
        {
            Name        = dto.Name,
            Description = dto.Description,
            Type        = dto.Type,
            IsActive    = dto.IsActive
        };
        await _repo.AddAsync(s);
        await _uow.CompleteAsync();
        return Map(s);
    }

    public async Task<StatusDTO?> UpdateAsync(Guid id, StatusDTO dto)
    {
        var s = await _repo.FindByIdAsync(id);
        if (s is null) return null;

        s.Name        = dto.Name;
        s.Description = dto.Description;
        s.Type        = dto.Type;
        s.IsActive    = dto.IsActive;
        s.UpdatedAt   = DateTime.UtcNow;

        _repo.Update(s);
        await _uow.CompleteAsync();
        return Map(s);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var s = await _repo.FindByIdAsync(id);
        if (s is null) return false;

        _repo.Remove(s);
        await _uow.CompleteAsync();
        return true;
    }
}
