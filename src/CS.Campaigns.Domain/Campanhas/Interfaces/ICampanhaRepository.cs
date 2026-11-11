using CS.Campaigns.Domain.Campanhas.Entities;
using CS.Campaigns.Domain.Common;

namespace CS.Campaigns.Domain.Campanhas.Interfaces;

public interface ICampanhaRepository
{
    Task AddAsync(CampanhaEntity campanha, CancellationToken ct = default);
    Task<CampanhaEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<CampanhaEntity>> ListAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CampanhaEntity>> ListByStatusAsync(StatusCampanha status, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
