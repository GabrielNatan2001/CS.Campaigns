using CS.Campaigns.Domain.Campanhas.Entities;
using CS.Campaigns.Domain.Campanhas.Interfaces;
using CS.Campaigns.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CS.Campaigns.Infrastructure.Data.Repositories;

public class CampanhaRepository : ICampanhaRepository
{
    private readonly AppDbContext _db;
    public CampanhaRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(CampanhaEntity campanha, CancellationToken ct = default)
        => await _db.Campanhas.AddAsync(campanha, ct);

    public Task<CampanhaEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Campanhas.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<CampanhaEntity>> ListAsync(CancellationToken ct = default)
        => await _db.Campanhas.OrderByDescending(c => c.CriadoEm).ToListAsync(ct);

    public async Task<IReadOnlyList<CampanhaEntity>> ListByStatusAsync(StatusCampanha status, CancellationToken ct = default)
        => await _db.Campanhas.Where(c => c.Status == status).OrderByDescending(c => c.CriadoEm).ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
