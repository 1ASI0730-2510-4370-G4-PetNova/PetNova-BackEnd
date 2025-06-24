using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Interfaces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _service;

    // Inyectamos la **interfaz**, NO la clase concreta
    public DoctorsController(IDoctorService service) => _service = service;

    // GET /api/Doctors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAll()
        => Ok(await _service.ListAsync());

    // GET /api/Doctors/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DoctorDTO>> Get(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    // POST /api/Doctors
    [HttpPost]
    public async Task<ActionResult<DoctorDTO>> Create([FromBody] DoctorDTO dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    // PUT /api/Doctors/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DoctorDTO>> Update(Guid id, [FromBody] DoctorDTO dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

         // DELETE /api/Doctors/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
