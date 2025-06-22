using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;

public interface IPetService
{
    Task<IEnumerable<PetDTO>>         ListAsync();
    Task<PetDTO?>                     GetByIdAsync(Guid id);
    Task<IEnumerable<PetDTO>>         GetByClientIdAsync(Guid clientId);
    Task<PetDTO>                      CreateAsync(PetDTO dto);
    Task<PetDTO?>                     UpdateAsync(Guid id, PetDTO dto);
    Task<bool>                        DeleteAsync(Guid id);
}