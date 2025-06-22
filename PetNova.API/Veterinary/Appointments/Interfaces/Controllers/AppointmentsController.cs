using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Appointments.Application.Services;
using PetNova.API.Veterinary.Appointments.Interfaces.DTOs;

namespace PetNova.API.Veterinary.Appointments.Interfaces.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AppointmentsController(IAppointmentService svc) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAll()
        => Ok(await svc.ListAsync());

    [HttpGet("{id:guid}")] public async Task<ActionResult<AppointmentDTO>> Get(Guid id)
        => (await svc.GetByIdAsync(id)) is { } dto ? Ok(dto) : NotFound();

    [HttpGet("pet/{petId:guid}")]
    public async Task<ActionResult<IEnumerable<AppointmentDTO>>> ByPet(Guid petId)
        => Ok(await svc.GetByPetIdAsync(petId));

    [HttpGet("doctor/{doctorId:guid}")]
    public async Task<ActionResult<IEnumerable<AppointmentDTO>>> ByDoctor(Guid doctorId)
        => Ok(await svc.GetByDoctorIdAsync(doctorId));

    [HttpPost]
    public async Task<ActionResult<AppointmentDTO>> Create([FromBody] AppointmentDTO dto)
    {
        var created = await svc.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AppointmentDTO>> Update(Guid id, [FromBody] AppointmentDTO dto)
        => (await svc.UpdateAsync(id, dto)) is { } upd ? Ok(upd) : NotFound();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => await svc.DeleteAsync(id) ? NoContent() : NotFound();
}
