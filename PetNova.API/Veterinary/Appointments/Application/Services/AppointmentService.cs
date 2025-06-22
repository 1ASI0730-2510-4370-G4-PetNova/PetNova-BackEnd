using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Interfaces.DTOs;
using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.Appointments.Application.Services;

public sealed class AppointmentService : IAppointmentService
{
    private readonly IRepository<Appointment, Guid> _repo;
    private readonly IRepository<Pet, Guid>         _pets;
    private readonly IRepository<Doctor, Guid>      _doctors;
    private readonly IUnitOfWork                    _uow;

    public AppointmentService(
        IRepository<Appointment, Guid> repo,
        IRepository<Pet, Guid>         pets,
        IRepository<Doctor, Guid>      doctors,
        IUnitOfWork                    uow)
    {
        _repo    = repo;
        _pets    = pets;
        _doctors = doctors;
        _uow     = uow;
    }

    /* ---------- mapping ---------- */
    private static AppointmentDTO Map(Appointment a) => new()
    {
        Id              = a.Id,
        AppointmentDate = a.AppointmentDate,
        Reason          = a.Reason,
        Status          = a.Status,
        Notes           = a.Notes,
        PetId           = a.PetId,
        DoctorId        = a.DoctorId
    };

    /* ---------- CRUD ---------- */

    public async Task<IEnumerable<AppointmentDTO>> ListAsync() =>
        (await _repo.ListAsync()).Select(Map);

    public async Task<AppointmentDTO?> GetByIdAsync(Guid id) =>
        (await _repo.FindByIdAsync(id)) is { } a ? Map(a) : null;

    public async Task<IEnumerable<AppointmentDTO>> GetByPetIdAsync(Guid petId) =>
        (await _repo.ListAsync())
        .Where(a => a.PetId == petId)
        .Select(Map);

    public async Task<IEnumerable<AppointmentDTO>> GetByDoctorIdAsync(Guid doctorId) =>
        (await _repo.ListAsync())
        .Where(a => a.DoctorId == doctorId)
        .Select(Map);

    public async Task<AppointmentDTO> CreateAsync(AppointmentDTO dto)
    {
        // Validar claves foráneas
        if (!await _pets.Exists(dto.PetId) || !await _doctors.Exists(dto.DoctorId))
            throw new InvalidOperationException("PetId o DoctorId no existen.");

        var a = new Appointment
        {
            AppointmentDate = dto.AppointmentDate,
            Reason          = dto.Reason,
            Status          = dto.Status,
            Notes           = dto.Notes,
            PetId           = dto.PetId,
            DoctorId        = dto.DoctorId
        };

        await _repo.AddAsync(a);
        await _uow.CompleteAsync();
        return Map(a);
    }

    public async Task<AppointmentDTO?> UpdateAsync(Guid id, AppointmentDTO dto)
    {
        var a = await _repo.FindByIdAsync(id);
        if (a is null) return null;

        a.AppointmentDate = dto.AppointmentDate;
        a.Reason          = dto.Reason;
        a.Status          = dto.Status;
        a.Notes           = dto.Notes;
        a.UpdatedAt       = DateTime.UtcNow;

        _repo.Update(a);
        await _uow.CompleteAsync();
        return Map(a);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var a = await _repo.FindByIdAsync(id);
        if (a is null) return false;

        _repo.Remove(a);
        await _uow.CompleteAsync();
        return true;
    }
}
