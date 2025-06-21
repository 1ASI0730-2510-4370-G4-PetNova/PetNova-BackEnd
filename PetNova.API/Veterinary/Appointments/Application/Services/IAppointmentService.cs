using PetNova.API.Veterinary.Appointments.Interfaces.DTOs;

namespace PetNova.API.Veterinary.Appointments.Application.Services;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDTO>> ListAsync();
    Task<AppointmentDTO?>             GetByIdAsync(Guid id);
    Task<IEnumerable<AppointmentDTO>> GetByPetIdAsync(Guid petId);
    Task<IEnumerable<AppointmentDTO>> GetByDoctorIdAsync(Guid doctorId);
    Task<AppointmentDTO>              CreateAsync(AppointmentDTO dto);
    Task<AppointmentDTO?>             UpdateAsync(Guid id, AppointmentDTO dto);
    Task<bool>                        DeleteAsync(Guid id);
}
