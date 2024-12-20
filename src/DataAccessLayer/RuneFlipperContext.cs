﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataAccessLayer;

public partial class RuneFlipperContext : IdentityDbContext<User>
{
    public RuneFlipperContext()
    {
    }

    public RuneFlipperContext(DbContextOptions<RuneFlipperContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BuyType> Buytypes { get; init; }

    public virtual DbSet<Character> Characters { get; init; }

    public virtual DbSet<Item> Items { get; init; }

    public virtual DbSet<Mode> Modes { get; init; }

    public virtual DbSet<SellType> Selltypes { get; init; }
    public virtual DbSet<Trade> Trades { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BuyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("buytypes_pkey");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("characters_pkey");

            entity.HasOne(d => d.Mode).WithMany(p => p.Characters).HasConstraintName("characters_modeid_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("items_pkey");

            entity.HasOne(d => d.Mode).WithMany(p => p.Items).HasConstraintName("items_modeid_fkey");
        });

        modelBuilder.Entity<Mode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("modes_pkey");
        });

        modelBuilder.Entity<SellType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("selltypes_pkey");
        });

        modelBuilder.Entity<Trade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trades_pkey");

            entity.HasOne(d => d.BuyType).WithMany(p => p.Trades).HasConstraintName("trades_buytypeid_fkey");

            entity.HasOne(d => d.Character).WithMany(p => p.Trades).HasConstraintName("trades_characterid_fkey");

            entity.HasOne(d => d.Item).WithMany(p => p.Trades).HasConstraintName("trades_itemid_fkey");

            entity.HasOne(d => d.SellType).WithMany(p => p.Trades).HasConstraintName("trades_selltypeid_fkey");
        });
    }
}
