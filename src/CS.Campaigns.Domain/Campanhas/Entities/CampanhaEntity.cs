using CS.Campaigns.Domain.Base;
using CS.Campaigns.Domain.Common;
using CS.Campaigns.Domain.Exceptions;

namespace CS.Campaigns.Domain.Campanhas.Entities;

public class CampanhaEntity : BaseEntity
{
    public string Titulo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public decimal MetaFinanceira { get; private set; }
    public StatusCampanha Status { get; private set; }
    public decimal ValorTotalArrecadado { get; private set; }
    public DateTime AtualizadoEm { get; private set; } = DateTime.UtcNow;

    protected CampanhaEntity() { }

    private CampanhaEntity(
        string titulo,
        string descricao,
        DateTime dataInicio,
        DateTime dataFim,
        decimal metaFinanceira,
        StatusCampanha status)
    {
        Titulo = titulo;
        Descricao = descricao;
        DataInicio = dataInicio;
        DataFim = dataFim;
        MetaFinanceira = metaFinanceira;
        Status = status;
        ValorTotalArrecadado = 0;
    }

    public static CampanhaEntity Criar(
        string titulo,
        string descricao,
        DateTime dataInicio,
        DateTime dataFim,
        decimal metaFinanceira,
        StatusCampanha status = StatusCampanha.Ativa)
    {
        Validar(titulo, descricao, dataInicio, dataFim, metaFinanceira, validarDataFimPassado: true);
        return new CampanhaEntity(titulo.Trim(), descricao.Trim(), dataInicio, dataFim, metaFinanceira, status);
    }

    public void Atualizar(
        string titulo,
        string descricao,
        DateTime dataInicio,
        DateTime dataFim,
        decimal metaFinanceira,
        StatusCampanha status)
    {
        Validar(titulo, descricao, dataInicio, dataFim, metaFinanceira, validarDataFimPassado: false);
        Titulo = titulo.Trim();
        Descricao = descricao.Trim();
        DataInicio = dataInicio;
        DataFim = dataFim;
        MetaFinanceira = metaFinanceira;
        Status = status;
        AtualizadoEm = DateTime.UtcNow;
    }

    public bool PodeReceberDoacao(DateTime utcNow)
        => Status == StatusCampanha.Ativa && utcNow <= DataFim;

    public void IncrementarArrecadacao(decimal valor)
    {
        if (valor <= 0)
            throw new DomainException("Valor deve ser maior que zero");
        ValorTotalArrecadado += valor;
        AtualizadoEm = DateTime.UtcNow;
    }

    private static void Validar(
        string titulo,
        string descricao,
        DateTime dataInicio,
        DateTime dataFim,
        decimal metaFinanceira,
        bool validarDataFimPassado)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Título é obrigatório");
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição é obrigatória");
        if (metaFinanceira <= 0)
            throw new DomainException("Meta financeira deve ser maior que zero");
        if (dataInicio > dataFim)
            throw new DomainException("Data início não pode ser posterior à data fim");
        if (validarDataFimPassado && dataFim < DateTime.UtcNow)
            throw new DomainException("Data de término não pode estar no passado");
    }
}
