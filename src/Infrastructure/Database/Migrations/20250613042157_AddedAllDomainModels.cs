using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedAllDomainModels : Migration
    {
        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0053:Use expression body for lambda expression", Justification = "<Pending>")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_on",
                schema: "public",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "buy_types",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    short_name = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_buy_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "modes",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    short_name = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sell_types",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    short_name = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sell_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "characters",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    is_member = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    mode_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_characters", x => x.id);
                    table.ForeignKey(
                        name: "fk_characters_modes_mode_id",
                        column: x => x.mode_id,
                        principalSchema: "public",
                        principalTable: "modes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_characters_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "items",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    in_game_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_members = table.Column<bool>(type: "boolean", nullable: false),
                    trade_limit = table.Column<int>(type: "integer", nullable: false),
                    mode_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_items_modes_mode_id",
                        column: x => x.mode_id,
                        principalSchema: "public",
                        principalTable: "modes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trades",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    buy_price = table.Column<long>(type: "bigint", nullable: false),
                    sell_price = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    buy_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sell_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_complete = table.Column<bool>(type: "boolean", nullable: false),
                    character_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<int>(type: "integer", nullable: false),
                    buy_type_id = table.Column<int>(type: "integer", nullable: false),
                    sell_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trades", x => x.id);
                    table.ForeignKey(
                        name: "fk_trades_buy_types_buy_type_id",
                        column: x => x.buy_type_id,
                        principalSchema: "public",
                        principalTable: "buy_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trades_characters_character_id",
                        column: x => x.character_id,
                        principalSchema: "public",
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trades_items_item_id",
                        column: x => x.item_id,
                        principalSchema: "public",
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trades_sell_types_sell_type_id",
                        column: x => x.sell_type_id,
                        principalSchema: "public",
                        principalTable: "sell_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_buy_types_short_name_name",
                schema: "public",
                table: "buy_types",
                columns: new[] { "short_name", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_characters_mode_id",
                schema: "public",
                table: "characters",
                column: "mode_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_user_id",
                schema: "public",
                table: "characters",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_items_mode_id",
                schema: "public",
                table: "items",
                column: "mode_id");

            migrationBuilder.CreateIndex(
                name: "ix_modes_short_name_name",
                schema: "public",
                table: "modes",
                columns: new[] { "short_name", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sell_types_short_name_name",
                schema: "public",
                table: "sell_types",
                columns: new[] { "short_name", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_trades_buy_type_id",
                schema: "public",
                table: "trades",
                column: "buy_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_trades_character_id",
                schema: "public",
                table: "trades",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_trades_item_id",
                schema: "public",
                table: "trades",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "ix_trades_sell_type_id",
                schema: "public",
                table: "trades",
                column: "sell_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trades",
                schema: "public");

            migrationBuilder.DropTable(
                name: "buy_types",
                schema: "public");

            migrationBuilder.DropTable(
                name: "characters",
                schema: "public");

            migrationBuilder.DropTable(
                name: "items",
                schema: "public");

            migrationBuilder.DropTable(
                name: "sell_types",
                schema: "public");

            migrationBuilder.DropTable(
                name: "modes",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "created_on",
                schema: "public",
                table: "users");
        }
    }
}
