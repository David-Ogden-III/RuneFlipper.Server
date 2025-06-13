using Domain.Items;
using Domain.Modes;
using Domain.Trades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Items;
internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne<Mode>().WithMany().HasForeignKey(x => x.ModeId);
        builder.HasMany<Trade>().WithOne().HasForeignKey(trade => trade.ItemId);
    }
}
