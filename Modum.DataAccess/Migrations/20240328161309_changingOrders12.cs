using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modum.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changingOrders12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("9599104c-13f7-4199-b4c1-d07b5fd647e5"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("b8dd9356-1e97-44a1-b732-eb74d2564b64"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("c99ad0b2-a3bf-4c4a-a480-e625e1bd57d3"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("fbf22f65-fe99-4ba6-bfd4-72818590f210"));

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "MainCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("48a2c277-7649-42a2-ab0f-c26bb5d049cc"), "Women" },
                    { new Guid("5b66ba9b-ff98-46ce-8713-c50cbb3dae65"), "Men" },
                    { new Guid("692d23ed-9e60-4694-90d2-f849f92d69a0"), "Boys" },
                    { new Guid("d2c5d468-1330-4deb-b80e-721ca6d6a053"), "Girls" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("48a2c277-7649-42a2-ab0f-c26bb5d049cc"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("5b66ba9b-ff98-46ce-8713-c50cbb3dae65"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("692d23ed-9e60-4694-90d2-f849f92d69a0"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("d2c5d468-1330-4deb-b80e-721ca6d6a053"));

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusOfDelivery = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_OrderStatus_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MainCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("9599104c-13f7-4199-b4c1-d07b5fd647e5"), "Girls" },
                    { new Guid("b8dd9356-1e97-44a1-b732-eb74d2564b64"), "Women" },
                    { new Guid("c99ad0b2-a3bf-4c4a-a480-e625e1bd57d3"), "Men" },
                    { new Guid("fbf22f65-fe99-4ba6-bfd4-72818590f210"), "Boys" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_OrderId",
                table: "OrderStatus",
                column: "OrderId");
        }
    }
}
