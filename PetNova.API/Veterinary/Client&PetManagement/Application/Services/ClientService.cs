using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;
using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;

public class ClientService:IClientService
{
    private readonly IRepository<Client, Guid> _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClientService(IRepository<Client, Guid> clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }
    private static ClientDTO MapToDto(Client c) => new()
    {
        Id        = c.Id,           // 👈 mapear Id
        FirstName = c.FirstName,
        LastName  = c.LastName,
        Email     = c.Email,
        Phone     = c.Phone,
        Address   = c.Address
    };


    public async Task<IEnumerable<ClientDTO>> ListAsync()
    {
        var clients = await _clientRepository.ListAsync();
        return clients.Select(MapToDto);
    }

    public async Task<ClientDTO?> GetByIdAsync(Guid id)
    {
        var client = await _clientRepository.FindByIdAsync(id);
        return client == null ? null : MapToDto(client);
    }



    public async Task<ClientDTO> CreateAsync(ClientDTO clientDto)
    {
        var client = new Client
        {
            FirstName = clientDto.FirstName,
            LastName = clientDto.LastName,
            Email = clientDto.Email,
            Phone = clientDto.Phone,
            Address = clientDto.Address
        };

        await _clientRepository.AddAsync(client);
        await _unitOfWork.CompleteAsync();

        return MapToDto(client);
    }

    public async Task<ClientDTO?> UpdateAsync(Guid id, ClientDTO clientDto)
    {
        var existingClient = await _clientRepository.FindByIdAsync(id);
        if (existingClient == null) return null;

        existingClient.FirstName = clientDto.FirstName;
        existingClient.LastName = clientDto.LastName;
        existingClient.Email = clientDto.Email;
        existingClient.Phone = clientDto.Phone;
        existingClient.Address = clientDto.Address;
        existingClient.UpdatedAt = DateTime.UtcNow;

        _clientRepository.Update(existingClient);
        await _unitOfWork.CompleteAsync();

        return MapToDto(existingClient);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var client = await _clientRepository.FindByIdAsync(id);
        if (client == null) return false;

        _clientRepository.Remove(client);
        await _unitOfWork.CompleteAsync();
        
        return true;
    }
}