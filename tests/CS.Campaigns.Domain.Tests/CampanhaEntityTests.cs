using CS.Campaigns.Domain.Campanhas.Entities;
using CS.Campaigns.Domain.Common;
using CS.Campaigns.Domain.Doacoes.Entities;
using CS.Campaigns.Domain.Exceptions;

namespace CS.Campaigns.Domain.Tests;

public class CampanhaEntityTests
{
    [Fact]
    public void Deve_rejeitar_meta_zero()
    {
        Assert.Throws<DomainException>(() =>
            CampanhaEntity.Criar("T", "D", DateTime.UtcNow, DateTime.UtcNow.AddDays(10), 0));
    }

    [Fact]
    public void Deve_rejeitar_data_fim_no_passado()
    {
        Assert.Throws<DomainException>(() =>
            CampanhaEntity.Criar("T", "D", DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-1), 100));
    }

    [Fact]
    public void Deve_bloquear_doacao_em_campanha_cancelada()
    {
        var c = CampanhaEntity.Criar("T", "D", DateTime.UtcNow, DateTime.UtcNow.AddDays(10), 100, StatusCampanha.Ativa);
        c.Atualizar("T", "D", DateTime.UtcNow, DateTime.UtcNow.AddDays(10), 100, StatusCampanha.Cancelada);
        Assert.Throws<DomainException>(() => DoacaoEntity.CriarIntencao(c, Guid.NewGuid(), 50));
    }

    [Fact]
    public void Deve_criar_doacao_pendente_em_campanha_ativa()
    {
        var c = CampanhaEntity.Criar("Natal", "Cestas", DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 50000);
        var d = DoacaoEntity.CriarIntencao(c, Guid.NewGuid(), 100);
        Assert.Equal(StatusDoacao.Pendente, d.Status);
        Assert.Equal(0, c.ValorTotalArrecadado);
    }
}
