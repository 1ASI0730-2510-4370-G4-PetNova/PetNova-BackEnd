using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;
using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;

public interface IClientService
{
    Task<IEnumerable<ClientDTO>> ListAsync();
    Task<ClientDTO?> GetByIdAsync(Guid id);
    Task<ClientDTO> CreateAsync(ClientDTO clientDto);
    Task<ClientDTO?> UpdateAsync(Guid id, ClientDTO clientDto);
    Task<bool> DeleteAsync(Guid id);
}