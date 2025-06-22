namespace PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

// DoctorDTO.cs
public class DoctorDTO
{
    public Guid   Id            { get; set; }
    public string FirstName     { get; set; } = default!;
    public string LastName      { get; set; } = default!;
    public string Specialization{ get; set; } = default!;
    public string LicenseNumber { get; set; } = default!;
    public string Email         { get; set; } = default!;
    public string Phone         { get; set; } = default!;
}
