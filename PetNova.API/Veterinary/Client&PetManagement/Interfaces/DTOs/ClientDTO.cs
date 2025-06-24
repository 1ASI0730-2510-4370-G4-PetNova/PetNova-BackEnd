namespace PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

public class ClientDTO
{
    public Guid   Id        { get; set; }  
    public string FirstName { get; set; } = default!;
    public string LastName  { get; set; } = default!;
    public string Email     { get; set; } = default!;
    public string Phone     { get; set; } = default!;
    public string Address   { get; set; } = default!;
}
