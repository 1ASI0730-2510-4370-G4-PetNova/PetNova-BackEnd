namespace PetNova.API.Veterinary.Appointments.Interfaces.DTOs;

public class AppointmentDTO
{
    public Guid     Id              { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string   Reason          { get; set; } = null!;
    public string   Status          { get; set; } = "Scheduled";
    public string   Notes           { get; set; } = null!;
    public Guid     PetId           { get; set; }
    public Guid     DoctorId        { get; set; }
}