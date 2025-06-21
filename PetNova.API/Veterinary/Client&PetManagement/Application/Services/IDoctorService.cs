using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDTO>> ListAsync();
    Task<DoctorDTO?>             GetByIdAsync(Guid id);
    Task<DoctorDTO>              CreateAsync(DoctorDTO dto);
    Task<DoctorDTO?>             UpdateAsync(Guid id, DoctorDTO dto);
    Task<bool>                   DeleteAsync(Guid id);
}