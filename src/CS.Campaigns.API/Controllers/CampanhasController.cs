using CS.Campaigns.Application.Campanhas.Dtos;
using CS.Campaigns.Application.Campanhas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CS.Campaigns.API.Controllers;

[ApiController]
[Route("api/campanhas")]
public class CampanhasController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "GestorONG")]
    public async Task<IActionResult> Criar([FromBody] CriarCampanhaDto dto, [FromServices] CriarCampanhaService service, CancellationToken ct)
    {
        var id = await service.ExecutarAsync(dto, ct);
        return CreatedAtAction(nameof(Criar), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "GestorONG")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarCampanhaDto dto, [FromServices] AtualizarCampanhaService service, CancellationToken ct)
    {
        await service.ExecutarAsync(id, dto, ct);
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "GestorONG")]
    public async Task<IActionResult> Listar([FromServices] ListarCampanhasService service, CancellationToken ct)
        => Ok(await service.ExecutarAsync(ct));

    [HttpGet("ativas")]
    [AllowAnonymous]
    public async Task<IActionResult> ListarAtivas([FromServices] ListarCampanhasAtivasService service, CancellationToken ct)
        => Ok(await service.ExecutarAsync(ct));
}
