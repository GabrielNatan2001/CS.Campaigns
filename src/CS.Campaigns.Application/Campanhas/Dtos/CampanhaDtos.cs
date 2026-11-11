using CS.Campaigns.Domain.Common;

namespace CS.Campaigns.Application.Campanhas.Dtos;

public record CriarCampanhaDto(
    string Titulo,
    string Descricao,
    DateTime DataInicio,
    DateTime DataFim,
    decimal MetaFinanceira,
    StatusCampanha Status);

public record AtualizarCampanhaDto(
    string Titulo,
    string Descricao,
    DateTime DataInicio,
    DateTime DataFim,
    decimal MetaFinanceira,
    StatusCampanha Status);

public record CampanhaGestaoDto(
    Guid Id,
    string Titulo,
    string Descricao,
    DateTime DataInicio,
    DateTime DataFim,
    decimal MetaFinanceira,
    string Status,
    decimal ValorTotalArrecadado);

public record CampanhaPublicaDto(
    string Titulo,
    decimal MetaFinanceira,
    decimal ValorTotalArrecadado);

public record CriarDoacaoDto(Guid CampanhaId, decimal ValorDoacao);

public record DoacaoCriadaDto(Guid DoacaoId);
