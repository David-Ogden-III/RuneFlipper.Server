using Domain.BuyTypes;
using Domain.Characters;
using Domain.Items;
using Domain.SellTypes;
using Domain.Trades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Trades;
internal sealed class TradeConfiguration : IEntityTypeConfiguration<Trade>
{
    public void Configure(EntityTypeBuilder<Trade> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne<BuyType>().WithMany().HasForeignKey(b => b.BuyTypeId);
        builder.HasOne<SellType>().WithMany().HasForeignKey(b => b.SellTypeId);
        builder.HasOne<Character>().WithMany().HasForeignKey(b => b.CharacterId);
        builder.HasOne<Item>().WithMany().HasForeignKey(b => b.ItemId);
    }
}
