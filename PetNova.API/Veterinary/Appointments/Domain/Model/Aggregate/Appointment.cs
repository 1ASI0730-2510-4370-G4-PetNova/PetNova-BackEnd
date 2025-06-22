using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;

public class Appointment
{
    public Guid Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; } = "Scheduled";
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