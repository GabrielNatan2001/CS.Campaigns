using CS.Campaigns.Application.Campanhas.Dtos;
using CS.Campaigns.Application.Messaging;
using CS.Campaigns.Application.Messaging.Events;
using CS.Campaigns.Domain.Campanhas.Entities;
using CS.Campaigns.Domain.Campanhas.Interfaces;
using CS.Campaigns.Domain.Common;
using CS.Campaigns.Domain.Doacoes.Entities;
using CS.Campaigns.Domain.Doacoes.Interfaces;
using CS.Campaigns.Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace CS.Campaigns.Application.Campanhas.Services;

public class CriarCampanhaService
{
    private readonly ICampanhaRepository _repo;
    public CriarCampanhaService(ICampanhaRepository repo) => _repo = repo;

    public async Task<Guid> ExecutarAsync(CriarCampanhaDto dto, CancellationToken ct = default)
    {
        var campanha = CampanhaEntity.Criar(
            dto.Titulo, dto.Descricao, dto.DataInicio, dto.DataFim, dto.MetaFinanceira, dto.Status);
        await _repo.AddAsync(campanha, ct);
        await _repo.SaveChangesAsync(ct);
        return campanha.Id;
    }
}

public class AtualizarCampanhaService
{
    private readonly ICampanhaRepository _repo;
    public AtualizarCampanhaService(ICampanhaRepository repo) => _repo = repo;

    public async Task ExecutarAsync(Guid id, AtualizarCampanhaDto dto, CancellationToken ct = default)
    {
        var campanha = await _repo.GetByIdAsync(id, ct)
            ?? throw new DomainException("Campanha não encontrada");
        campanha.Atualizar(dto.Titulo, dto.Descricao, dto.DataInicio, dto.DataFim, dto.MetaFinanceira, dto.Status);
        await _repo.SaveChangesAsync(ct);
    }
}

public class ListarCampanhasService
{
    private readonly ICampanhaRepository _repo;
    public ListarCampanhasService(ICampanhaRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<CampanhaGestaoDto>> ExecutarAsync(CancellationToken ct = default)
    {
        var list = await _repo.ListAsync(ct);
        return list.Select(c => new CampanhaGestaoDto(
            c.Id, c.Titulo, c.Descricao, c.DataInicio, c.DataFim,
            c.MetaFinanceira, c.Status.ToString(), c.ValorTotalArrecadado)).ToList();
    }
}

public class ListarCampanhasAtivasService
{
    private readonly ICampanhaRepository _repo;
    public ListarCampanhasAtivasService(ICampanhaRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<CampanhaPublicaDto>> ExecutarAsync(CancellationToken ct = default)
    {
        var list = await _repo.ListByStatusAsync(StatusCampanha.Ativa, ct);
        return list.Select(c => new CampanhaPublicaDto(c.Titulo, c.MetaFinanceira, c.ValorTotalArrecadado)).ToList();
    }
}

public class CriarDoacaoService
{
    private readonly ICampanhaRepository _campanhas;
    private readonly IDoacaoRepository _doacoes;
    private readonly IMessageBus _bus;
    private readonly DoacaoRecebidaPublisherConfig _publisher;

    public CriarDoacaoService(
        ICampanhaRepository campanhas,
        IDoacaoRepository doacoes,
        IMessageBus bus,
        IOptions<DoacaoRecebidaPublisherConfig> publisher)
    {
        _campanhas = campanhas;
        _doacoes = doacoes;
        _bus = bus;
        _publisher = publisher.Value;
    }

    public async Task<DoacaoCriadaDto> ExecutarAsync(Guid doadorId, CriarDoacaoDto dto, CancellationToken ct = default)
    {
        var campanha = await _campanhas.GetByIdAsync(dto.CampanhaId, ct)
            ?? throw new DomainException("Campanha não encontrada");

        var doacao = DoacaoEntity.CriarIntencao(campanha, doadorId, dto.ValorDoacao);
        await _doacoes.AddAsync(doacao, ct);
        await _doacoes.SaveChangesAsync(ct);

        // API NÃO atualiza ValorTotalArrecadado — apenas publica evento
        _bus.Publish(
            _publisher.Exchange,
            _publisher.RoutingKey,
            new DoacaoRecebidaEvent(doacao.Id, doacao.CampanhaId, doacao.DoadorId, doacao.Valor, DateTime.UtcNow));

        return new DoacaoCriadaDto(doacao.Id);
    }
}
