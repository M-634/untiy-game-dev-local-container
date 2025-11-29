using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pj_server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMasterTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MItems",
                columns: table => new
                {
                    item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    item_type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    max_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MItems", x => x.item_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MItemLocalizes",
                columns: table => new
                {
                    item_id = table.Column<int>(type: "int", nullable: false),
                    text = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MItemLocalizes", x => x.item_id);
                    table.ForeignKey(
                        name: "FK_MItemLocalizes_MItems_item_id",
                        column: x => x.item_id,
                        principalTable: "MItems",
                        principalColumn: "item_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MItemLocalizes");

            migrationBuilder.DropTable(
                name: "MItems");
        }
    }
}
