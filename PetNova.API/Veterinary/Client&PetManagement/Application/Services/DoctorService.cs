using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepository<Doctor, Guid> _repo;
    private readonly IUnitOfWork               _uow;

    public DoctorService(IRepository<Doctor, Guid> repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    // ----------  Mapping helpers ----------
    private static DoctorDTO MapToDto(Doctor d) => new()
    {
        Id             = d.Id,
        FirstName      = d.FirstName,
        LastName       = d.LastName,
        Specialization = d.Specialization,
        LicenseNumber  = d.LicenseNumber,
        Email          = d.Email,
        Phone          = d.Phone
    };

    private static void MapFromDto(Doctor d, DoctorDTO dto)
    {
        d.FirstName      = dto.FirstName;
        d.LastName       = dto.LastName;
        d.Specialization = dto.Specialization;
        d.LicenseNumber  = dto.LicenseNumber;
        d.Email          = dto.Email;
        d.Phone          = dto.Phone;
    }

    // ----------  Queries ----------
    public async Task<IEnumerable<DoctorDTO>> ListAsync() =>
        (await _repo.ListAsync()).Select(MapToDto);

    public async Task<DoctorDTO?> GetByIdAsync(Guid id) =>
        (await _repo.FindByIdAsync(id)) is { } d ? MapToDto(d) : null;

    // ----------  Commands ----------
    public async Task<DoctorDTO> CreateAsync(DoctorDTO dto)
    {
        var entity = new Doctor();
        MapFromDto(entity, dto);

        await _repo.AddAsync(entity);
        await _uow.CompleteAsync();

        return MapToDto(entity);
    }

    public async Task<DoctorDTO?> UpdateAsync(Guid id, DoctorDTO dto)
    {
        var entity = await _repo.FindByIdAsync(id);
        if (entity is null) return null;

        MapFromDto(entity, dto);
        entity.UpdatedAt = DateTime.UtcNow;

        _repo.Update(entity);
        await _uow.CompleteAsync();

        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _repo.FindByIdAsync(id);
        if (entity is null) return false;

        _repo.Remove(entity);
        await _uow.CompleteAsync();
        return true;
    }
}
