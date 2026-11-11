using System.Security.Claims;
using CS.Campaigns.Application.Campanhas.Dtos;
using CS.Campaigns.Application.Campanhas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CS.Campaigns.API.Controllers;

[ApiController]
[Route("api/doacoes")]
public class DoacoesController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Doador")]
    public async Task<IActionResult> Criar([FromBody] CriarDoacaoDto dto, [FromServices] CriarDoacaoService service, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Usuário não identificado no token");
        var result = await service.ExecutarAsync(Guid.Parse(userId), dto, ct);
        return Accepted(result);
    }
}
