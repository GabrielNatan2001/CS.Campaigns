using CS.Campaigns.Domain.Doacoes.Entities;
using CS.Campaigns.Domain.Doacoes.Interfaces;

namespace CS.Campaigns.Infrastructure.Data.Repositories;

public class DoacaoRepository : IDoacaoRepository
{
    private readonly AppDbContext _db;
    public DoacaoRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(DoacaoEntity doacao, CancellationToken ct = default)
        => await _db.Doacoes.AddAsync(doacao, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
