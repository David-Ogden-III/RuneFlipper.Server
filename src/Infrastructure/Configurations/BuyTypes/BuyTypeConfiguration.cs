using Domain.BuyTypes;
using Domain.Trades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.BuyTypes;
internal sealed class BuyTypeConfiguration : IEntityTypeConfiguration<BuyType>
{
    public void Configure(EntityTypeBuilder<BuyType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.ShortName, x.Name }).IsUnique();
        builder.HasMany<Trade>().WithOne().HasForeignKey(trade => trade.BuyTypeId);
    }
}
