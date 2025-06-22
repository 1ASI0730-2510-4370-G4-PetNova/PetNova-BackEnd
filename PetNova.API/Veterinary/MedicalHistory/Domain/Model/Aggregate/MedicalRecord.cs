using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.MedicalHistory.Domain.Model.Aggregate;

public class MedicalRecord
{
    public Guid Id { get; set; }
    public DateTime RecordDate { get; set; }
    public string Diagnosis { get; set; }
    public string Treatment { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign keys
    public Guid PetId { get; set; }
    public Guid DoctorId { get; set; }
    
    // Navigation properties
    public Pet Pet { get; set; }
    public Doctor Doctor { get; set; }
}