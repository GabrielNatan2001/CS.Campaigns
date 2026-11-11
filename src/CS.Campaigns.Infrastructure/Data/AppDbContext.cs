using CS.Campaigns.Domain.Campanhas.Entities;
using CS.Campaigns.Domain.Doacoes.Entities;
using Microsoft.EntityFrameworkCore;

namespace CS.Campaigns.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CampanhaEntity> Campanhas => Set<CampanhaEntity>();
    public DbSet<DoacaoEntity> Doacoes => Set<DoacaoEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
