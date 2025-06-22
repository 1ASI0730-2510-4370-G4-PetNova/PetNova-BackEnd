namespace PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;

public class Client
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation property for Pets
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}