using CS.Campaigns.Domain.Base;
using CS.Campaigns.Domain.Campanhas.Entities;
using CS.Campaigns.Domain.Common;
using CS.Campaigns.Domain.Exceptions;

namespace CS.Campaigns.Domain.Doacoes.Entities;

public class DoacaoEntity : BaseEntity
{
    public Guid CampanhaId { get; private set; }
    public Guid DoadorId { get; private set; }
    public decimal Valor { get; private set; }
    public StatusDoacao Status { get; private set; }

    protected DoacaoEntity() { }

    private DoacaoEntity(Guid campanhaId, Guid doadorId, decimal valor)
    {
        CampanhaId = campanhaId;
        DoadorId = doadorId;
        Valor = valor;
        Status = StatusDoacao.Pendente;
    }

    public static DoacaoEntity CriarIntencao(CampanhaEntity campanha, Guid doadorId, decimal valor)
    {
        if (valor <= 0)
            throw new DomainException("Valor da doação deve ser maior que zero");
        if (!campanha.PodeReceberDoacao(DateTime.UtcNow))
            throw new DomainException("Doação não pode ser feita para campanhas encerradas ou canceladas");

        return new DoacaoEntity(campanha.Id, doadorId, valor);
    }

    public void MarcarProcessada()
    {
        if (Status == StatusDoacao.Processada)
            return;
        Status = StatusDoacao.Processada;
    }
}
