using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pj_server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMItemGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "u_user",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "u_user",
                newName: "UserId");

            migrationBuilder.CreateTable(
                name: "m_item_group",
                columns: table => new
                {
                    ItemGroupId = table.Column<int>(type: "int", nullable: false),
                    ItemGroupName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_item_group", x => x.ItemGroupId);
                    table.ForeignKey(
                        name: "FK_m_item_group_m_item_ItemGroupId",
                        column: x => x.ItemGroupId,
                        principalTable: "m_item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_item_group");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "u_user",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "u_user",
                newName: "Id");
        }
    }
}
