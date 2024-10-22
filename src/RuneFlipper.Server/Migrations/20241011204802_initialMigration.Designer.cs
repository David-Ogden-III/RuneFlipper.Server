﻿// <auto-generated />
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuneFlipper.Server.Migrations
{
    [DbContext(typeof(RuneFlipperContext))]
    [Migration("20241011204802_initialMigration")]
    partial class initialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RuneFlipper.Server.Models.Buytype", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("buytypes_pkey");

                    b.HasIndex(new[] { "Name" }, "buytypes_name_key")
                        .IsUnique();

                    b.ToTable("buytypes");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Character", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("Createdat")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdat");

                    b.Property<bool>("Member")
                        .HasColumnType("boolean")
                        .HasColumnName("member");

                    b.Property<string>("Modeid")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("modeid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("characters_pkey");

                    b.HasIndex("Modeid");

                    b.ToTable("characters");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Item", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("Ingameid")
                        .HasColumnType("integer")
                        .HasColumnName("ingameid");

                    b.Property<bool>("Member")
                        .HasColumnType("boolean")
                        .HasColumnName("member");

                    b.Property<string>("Modeid")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("modeid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<int>("Tradelimit")
                        .HasColumnType("integer")
                        .HasColumnName("tradelimit");

                    b.HasKey("Id")
                        .HasName("items_pkey");

                    b.HasIndex("Modeid");

                    b.ToTable("items");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Mode", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("modes_pkey");

                    b.HasIndex(new[] { "Name" }, "modes_name_key")
                        .IsUnique();

                    b.ToTable("modes");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Selltype", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("selltypes_pkey");

                    b.HasIndex(new[] { "Name" }, "selltypes_name_key")
                        .IsUnique();

                    b.ToTable("selltypes");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Trade", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("Buydatetime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("buydatetime");

                    b.Property<long>("Buyprice")
                        .HasColumnType("bigint")
                        .HasColumnName("buyprice");

                    b.Property<string>("Buytypeid")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("buytypeid");

                    b.Property<string>("Characterid")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("characterid");

                    b.Property<bool>("Iscomplete")
                        .HasColumnType("boolean")
                        .HasColumnName("iscomplete");

                    b.Property<string>("Itemid")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("itemid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<DateTime>("Selldatetime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("selldatetime");

                    b.Property<long>("Sellprice")
                        .HasColumnType("bigint")
                        .HasColumnName("sellprice");

                    b.Property<string>("Selltypeid")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("selltypeid");

                    b.HasKey("Id")
                        .HasName("trades_pkey");

                    b.HasIndex("Buytypeid");

                    b.HasIndex("Characterid");

                    b.HasIndex("Itemid");

                    b.HasIndex("Selltypeid");

                    b.ToTable("trades");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Character", b =>
                {
                    b.HasOne("RuneFlipper.Server.Models.Mode", "Mode")
                        .WithMany("Characters")
                        .HasForeignKey("Modeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("characters_modeid_fkey");

                    b.Navigation("Mode");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Item", b =>
                {
                    b.HasOne("RuneFlipper.Server.Models.Mode", "Mode")
                        .WithMany("Items")
                        .HasForeignKey("Modeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("items_modeid_fkey");

                    b.Navigation("Mode");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Trade", b =>
                {
                    b.HasOne("RuneFlipper.Server.Models.Buytype", "Buytype")
                        .WithMany("Trades")
                        .HasForeignKey("Buytypeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("trades_buytypeid_fkey");

                    b.HasOne("RuneFlipper.Server.Models.Character", "Character")
                        .WithMany("Trades")
                        .HasForeignKey("Characterid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("trades_characterid_fkey");

                    b.HasOne("RuneFlipper.Server.Models.Item", "Item")
                        .WithMany("Trades")
                        .HasForeignKey("Itemid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("trades_itemid_fkey");

                    b.HasOne("RuneFlipper.Server.Models.Selltype", "Selltype")
                        .WithMany("Trades")
                        .HasForeignKey("Selltypeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("trades_selltypeid_fkey");

                    b.Navigation("Buytype");

                    b.Navigation("Character");

                    b.Navigation("Item");

                    b.Navigation("Selltype");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Buytype", b =>
                {
                    b.Navigation("Trades");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Character", b =>
                {
                    b.Navigation("Trades");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Item", b =>
                {
                    b.Navigation("Trades");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Mode", b =>
                {
                    b.Navigation("Characters");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("RuneFlipper.Server.Models.Selltype", b =>
                {
                    b.Navigation("Trades");
                });
#pragma warning restore 612, 618
        }
    }
}
