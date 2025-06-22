using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Status.Application.Services;
using PetNova.API.Veterinary.Status.Interface.DTOs;

namespace PetNova.API.Veterinary.Status.Interface.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StatusController(IStatusService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatusDTO>>> GetAll()
        => Ok(await service.ListAsync());

    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<StatusDTO>>> GetByType(string type)
        => Ok(await service.ListByTypeAsync(type));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StatusDTO>> GetById(Guid id)
        => (await service.GetByIdAsync(id)) is { } dto ? Ok(dto) : NotFound();

    [HttpPost]
    public async Task<ActionResult<StatusDTO>> Create([FromBody] StatusDTO dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<StatusDTO>> Update(Guid id, [FromBody] StatusDTO dto)
        => (await service.UpdateAsync(id, dto)) is { } upd ? Ok(upd) : NotFound();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => await service.DeleteAsync(id) ? NoContent() : NotFound();
}