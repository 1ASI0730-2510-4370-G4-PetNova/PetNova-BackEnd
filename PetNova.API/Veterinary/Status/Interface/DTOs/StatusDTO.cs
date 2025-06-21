namespace PetNova.API.Veterinary.Status.Interface.DTOs;

public class StatusDTO
{
    public Guid   Id          { get; set; }          // ← NUEVO
    public string Name        { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type        { get; set; } = null!; // Appointment | Pet | …
    public bool   IsActive    { get; set; } = true;
}