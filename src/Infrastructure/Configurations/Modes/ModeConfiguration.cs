using Domain.Characters;
using Domain.Items;
using Domain.Modes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Modes;
internal sealed class ModeConfiguration : IEntityTypeConfiguration<Mode>
{
    public void Configure(EntityTypeBuilder<Mode> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.ShortName, x.Name }).IsUnique();
        builder.HasMany<Character>().WithOne().HasForeignKey(c => c.ModeId);
        builder.HasMany<Item>().WithOne().HasForeignKey(c => c.ModeId);
    }
}
