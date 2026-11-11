using CS.Campaigns.Domain.Doacoes.Entities;

namespace CS.Campaigns.Domain.Doacoes.Interfaces;

public interface IDoacaoRepository
{
    Task AddAsync(DoacaoEntity doacao, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
