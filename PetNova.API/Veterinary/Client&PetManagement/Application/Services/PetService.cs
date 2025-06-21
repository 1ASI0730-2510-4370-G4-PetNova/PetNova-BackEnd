using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;

public class PetService : IPetService
{
    private readonly IRepository<Pet, Guid> _petRepository;
    private readonly IRepository<Client, Guid> _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PetService(
        IRepository<Pet, Guid> petRepository,
        IRepository<Client, Guid> clientRepository,
        IUnitOfWork unitOfWork)
    {
        _petRepository = petRepository;
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    private static PetDTO Map(Pet pet) => new()
    {
        Id           = pet.Id,
        Name         = pet.Name,
        Species      = pet.Species,
        Breed        = pet.Breed,
        DateOfBirth  = pet.DateOfBirth,
        Color        = pet.Color,
        Gender       = pet.Gender,
        MicrochipId  = pet.MicrochipId,
        ClientId     = pet.ClientId
    };

    public async Task<IEnumerable<PetDTO>> ListAsync()
        => (await _petRepository.ListAsync()).Select(Map);

    public async Task<PetDTO?> GetByIdAsync(Guid id)
        => (await _petRepository.FindByIdAsync(id)) is { } pet ? Map(pet) : null;

    public async Task<IEnumerable<PetDTO>> GetByClientIdAsync(Guid clientId)
    {
        var pets = (await _petRepository.ListAsync()).Where(p => p.ClientId == clientId);
        return pets.Select(Map);
    }

    public async Task<PetDTO> CreateAsync(PetDTO dto)
    {
        var pet = new Pet
        {
            Name         = dto.Name,
            Species      = dto.Species,
            Breed        = dto.Breed,
            DateOfBirth  = dto.DateOfBirth,
            Color        = dto.Color,
            Gender       = dto.Gender,
            MicrochipId  = dto.MicrochipId,
            ClientId     = dto.ClientId
        };

        await _petRepository.AddAsync(pet);
        await _unitOfWork.CompleteAsync();
        return Map(pet);
    }

    public async Task<PetDTO?> UpdateAsync(Guid id, PetDTO dto)
    {
        var pet = await _petRepository.FindByIdAsync(id);
        if (pet is null) return null;

        pet.Name        = dto.Name;
        pet.Species     = dto.Species;
        pet.Breed       = dto.Breed;
        pet.DateOfBirth = dto.DateOfBirth;
        pet.Color       = dto.Color;
        pet.Gender      = dto.Gender;
        pet.MicrochipId = dto.MicrochipId;
        pet.UpdatedAt   = DateTime.UtcNow;

        _petRepository.Update(pet);
        await _unitOfWork.CompleteAsync();

        return Map(pet);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var pet = await _petRepository.FindByIdAsync(id);
        if (pet is null) return false;

        _petRepository.Remove(pet);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
