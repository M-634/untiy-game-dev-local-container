using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedWithHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "m_item",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.InsertData(
                table: "m_item_group",
                columns: new[] { "ItemGroupId", "CreatedAt", "ItemGroupName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3480), "回復薬", new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3480) },
                    { 2, new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3480), "武器", new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3480) }
                });

            migrationBuilder.InsertData(
                table: "m_item",
                columns: new[] { "ItemId", "CreatedAt", "ItemGroupId", "MaxPossessCount", "Rarity" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3500), 1, 99, 1 },
                    { 2, new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3500), 1, 99, 2 },
                    { 3, new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3500), 2, 99, 5 }
                });

            migrationBuilder.InsertData(
                table: "m_item_localize",
                columns: new[] { "ItemId", "CreatedAt", "ItemName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3510), "アイテムテスト1", new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3510) },
                    { 2, new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3510), "アイテムテスト2", new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3510) },
                    { 3, new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3510), "アイテムテスト3", new DateTime(2025, 11, 5, 17, 59, 52, 911, DateTimeKind.Utc).AddTicks(3510) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "m_item_localize",
                keyColumn: "ItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "m_item_localize",
                keyColumn: "ItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "m_item_localize",
                keyColumn: "ItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "m_item",
                keyColumn: "ItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "m_item",
                keyColumn: "ItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "m_item",
                keyColumn: "ItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "m_item_group",
                keyColumn: "ItemGroupId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "m_item_group",
                keyColumn: "ItemGroupId",
                keyValue: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "m_item",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);
        }
    }
}
