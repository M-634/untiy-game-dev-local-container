using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationsAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_m_item_group_m_item_ItemGroupId",
                table: "m_item_group");

            migrationBuilder.DropForeignKey(
                name: "FK_m_item_localize_m_item_ItemId",
                table: "m_item_localize");

            migrationBuilder.DropForeignKey(
                name: "FK_u_user_item_m_item_ItemId",
                table: "u_user_item");

            migrationBuilder.DropForeignKey(
                name: "FK_u_user_item_u_user_UserId",
                table: "u_user_item");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "u_user_item",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "u_user_item",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "u_user",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "u_user",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "m_item_localize",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "m_item_localize",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "m_item_group",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "m_item_group",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "ItemGroupId",
                table: "m_item_group",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "m_item",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "m_item",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.CreateIndex(
                name: "IX_u_user_item_UserId",
                table: "u_user_item",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_m_item_ItemGroupId",
                table: "m_item",
                column: "ItemGroupId");

            migrationBuilder.AddForeignKey(
                name: "fk_m_item__item_group_id",
                table: "m_item",
                column: "ItemGroupId",
                principalTable: "m_item_group",
                principalColumn: "ItemGroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_m_item_localize__item_id",
                table: "m_item_localize",
                column: "ItemId",
                principalTable: "m_item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_u_user_item__item_id",
                table: "u_user_item",
                column: "ItemId",
                principalTable: "m_item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_u_user_item__user_id",
                table: "u_user_item",
                column: "UserId",
                principalTable: "u_user",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_m_item__item_group_id",
                table: "m_item");

            migrationBuilder.DropForeignKey(
                name: "fk_m_item_localize__item_id",
                table: "m_item_localize");

            migrationBuilder.DropForeignKey(
                name: "fk_u_user_item__item_id",
                table: "u_user_item");

            migrationBuilder.DropForeignKey(
                name: "fk_u_user_item__user_id",
                table: "u_user_item");

            migrationBuilder.DropIndex(
                name: "IX_u_user_item_UserId",
                table: "u_user_item");

            migrationBuilder.DropIndex(
                name: "IX_m_item_ItemGroupId",
                table: "m_item");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "m_item_localize");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "m_item_localize");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "u_user_item",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "u_user_item",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "u_user",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "u_user",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "m_item_group",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "m_item_group",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<int>(
                name: "ItemGroupId",
                table: "m_item_group",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "m_item",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "m_item",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddForeignKey(
                name: "FK_m_item_group_m_item_ItemGroupId",
                table: "m_item_group",
                column: "ItemGroupId",
                principalTable: "m_item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_m_item_localize_m_item_ItemId",
                table: "m_item_localize",
                column: "ItemId",
                principalTable: "m_item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_u_user_item_m_item_ItemId",
                table: "u_user_item",
                column: "ItemId",
                principalTable: "m_item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_u_user_item_u_user_UserId",
                table: "u_user_item",
                column: "UserId",
                principalTable: "u_user",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
