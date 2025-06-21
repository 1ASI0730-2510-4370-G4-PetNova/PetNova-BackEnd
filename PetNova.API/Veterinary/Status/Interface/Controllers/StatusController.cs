using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Status.Application.Services;
using PetNova.API.Veterinary.Status.Interface.DTOs;

namespace PetNova.API.Veterinary.Status.Interface.Controllers;

[ApiController]
[Route("api/[controller]")]
// Controller for managing Status entities
public sealed class StatusController(IStatusService service) : ControllerBase
{
    // GET: /api/status - Retrieve all status entries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatusDTO>>> GetAll()
        => Ok(await service.ListAsync());

    // GET: /api/status/type/{type} - Retrieve status entries by type
    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<StatusDTO>>> GetByType(string type)
        => Ok(await service.ListByTypeAsync(type));

    // GET: /api/status/{id} - Retrieve a single status entry by its ID
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StatusDTO>> GetById(Guid id)
        // If status is found, return Ok, otherwise return NotFound
        => (await service.GetByIdAsync(id)) is { } dto ? Ok(dto) : NotFound();

    // POST: /api/status - Create a new status entry
    [HttpPost]
    public async Task<ActionResult<StatusDTO>> Create([FromBody] StatusDTO dto)
    {
        // Create a new status and return a CreatedAtAction result with the newly created status details
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: /api/status/{id} - Update an existing status entry by ID
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<StatusDTO>> Update(Guid id, [FromBody] StatusDTO dto)
        // If status is updated, return Ok with updated status, otherwise return NotFound
        => (await service.UpdateAsync(id, dto)) is { } upd ? Ok(upd) : NotFound();

    // DELETE: /api/status/{id} - Delete a status entry by ID
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        // If deletion is successful, return NoContent, otherwise return NotFound
        => await service.DeleteAsync(id) ? NoContent() : NotFound();
}
