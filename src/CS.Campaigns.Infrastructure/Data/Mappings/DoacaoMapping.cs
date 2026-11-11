using CS.Campaigns.Domain.Doacoes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CS.Campaigns.Infrastructure.Data.Mappings;

public class DoacaoMapping : IEntityTypeConfiguration<DoacaoEntity>
{
    public void Configure(EntityTypeBuilder<DoacaoEntity> builder)
    {
        builder.ToTable("doacoes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CampanhaId).IsRequired();
        builder.Property(x => x.DoadorId).IsRequired();
        builder.Property(x => x.Valor).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(x => x.CriadoEm).IsRequired();
        builder.HasIndex(x => x.CampanhaId);
    }
}
