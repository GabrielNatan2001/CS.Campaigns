using CS.Campaigns.Domain.Campanhas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CS.Campaigns.Infrastructure.Data.Mappings;

public class CampanhaMapping : IEntityTypeConfiguration<CampanhaEntity>
{
    public void Configure(EntityTypeBuilder<CampanhaEntity> builder)
    {
        builder.ToTable("campanhas");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Titulo).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Descricao).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.DataInicio).IsRequired();
        builder.Property(x => x.DataFim).IsRequired();
        builder.Property(x => x.MetaFinanceira).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(x => x.ValorTotalArrecadado).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(x => x.CriadoEm).IsRequired();
        builder.Property(x => x.AtualizadoEm).IsRequired();
    }
}
