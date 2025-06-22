using PetNova.API.Veterinary.Status.Interface.DTOs;

namespace PetNova.API.Veterinary.Status.Application.Services;

public interface IStatusService
{
    Task<IEnumerable<StatusDTO>> ListAsync();
    Task<IEnumerable<StatusDTO>> ListByTypeAsync(string type);
    Task<StatusDTO?>             GetByIdAsync(Guid id);
    Task<StatusDTO>              CreateAsync(StatusDTO dto);
    Task<StatusDTO?>             UpdateAsync(Guid id, StatusDTO dto);
    Task<bool>                   DeleteAsync(Guid id);
}