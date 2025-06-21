namespace PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

public class PetDTO
{
    public Guid   Id          { get; set; }          
    public string Name        { get; set; } = default!;
    public string Species     { get; set; } = default!;
    public string Breed       { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string Color       { get; set; } = default!;
    public string Gender      { get; set; } = default!;
    public string MicrochipId { get; set; } = default!;
    public Guid   ClientId    { get; set; }       
}