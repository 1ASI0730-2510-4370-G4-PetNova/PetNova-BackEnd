using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.Client_PetManagement.Interfaces.Controllers;

/// <summary>
///  Endpoints CRUD para los clientes de la clínica.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
        => _clientService = clientService;

    // GET /api/Clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDTO>>> GetAll()
        => Ok(await _clientService.ListAsync());

    // GET /api/Clients/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClientDTO>> GetById(Guid id)
    {
        var dto = await _clientService.GetByIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    // POST /api/Clients
    [HttpPost]
    public async Task<ActionResult<ClientDTO>> Create([FromBody] ClientDTO dto)
    {
        var created = await _clientService.CreateAsync(dto);

        // IMPORTANTE: ClientDTO debe exponer la propiedad Id 
        // para que CreatedAtAction pueda generar correctamente la URL.
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/Clients/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ClientDTO>> Update(Guid id, [FromBody] ClientDTO dto)
    {
        var updated = await _clientService.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    // DELETE /api/Clients/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => await _clientService.DeleteAsync(id) ? NoContent() : NotFound();
}
