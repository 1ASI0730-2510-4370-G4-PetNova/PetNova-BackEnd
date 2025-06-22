using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.MedicalHistory.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;

public class Pet
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Color { get; set; }
    public string Gender { get; set; }
    public string MicrochipId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign key
    public Guid ClientId { get; set; }
    
    // Navigation properties
    public Client Client { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}