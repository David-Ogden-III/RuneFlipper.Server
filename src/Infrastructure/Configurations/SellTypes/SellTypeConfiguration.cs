using Domain.SellTypes;
using Domain.Trades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.SellTypes;
internal sealed class SellTypeConfiguration : IEntityTypeConfiguration<SellType>
{
    public void Configure(EntityTypeBuilder<SellType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.ShortName, x.Name }).IsUnique();
        builder.HasMany<Trade>().WithOne().HasForeignKey(t => t.SellTypeId);
    }
}
