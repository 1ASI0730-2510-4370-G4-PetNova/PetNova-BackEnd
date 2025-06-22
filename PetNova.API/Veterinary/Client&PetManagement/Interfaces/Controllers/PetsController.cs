using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.Client_PetManagement.Interfaces.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PetsController(IPetService service) : ControllerBase
{
    [HttpGet]                     public async Task<IActionResult> GetAll()                => Ok(await service.ListAsync());
    [HttpGet("{id:guid}")]        public async Task<IActionResult> Get(Guid id)            => 
        await service.GetByIdAsync(id) is { } dto ? Ok(dto) : NotFound();
    [HttpGet("client/{id:guid}")] public async Task<IActionResult> GetByClient(Guid id)    => Ok(await service.GetByClientIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PetDTO dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] PetDTO dto) =>
        await service.UpdateAsync(id, dto) is { } updated ? Ok(updated) : NotFound();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}