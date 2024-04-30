using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modum.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changingOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Orders_OrderId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_OrderId",
                table: "Product");

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("5ab46b4e-d90d-44a4-9d0d-0f6962afc623"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("89b19aa4-7181-4899-b59f-6fa1be711588"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("b056b89b-c5a2-48dd-ba08-f28d7e0001c6"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("b2c14561-a4d8-4fd2-9957-2ebedbafa325"));

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Product");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "ProductSizesHelpingTable",
                type: "uniqueidentifier",
                nullable: true);

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
                name: "IX_ProductSizesHelpingTable_OrderId",
                table: "ProductSizesHelpingTable",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizesHelpingTable_Orders_OrderId",
                table: "ProductSizesHelpingTable",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizesHelpingTable_Orders_OrderId",
                table: "ProductSizesHelpingTable");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizesHelpingTable_OrderId",
                table: "ProductSizesHelpingTable");

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

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductSizesHelpingTable");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "MainCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("5ab46b4e-d90d-44a4-9d0d-0f6962afc623"), "Boys" },
                    { new Guid("89b19aa4-7181-4899-b59f-6fa1be711588"), "Women" },
                    { new Guid("b056b89b-c5a2-48dd-ba08-f28d7e0001c6"), "Girls" },
                    { new Guid("b2c14561-a4d8-4fd2-9957-2ebedbafa325"), "Men" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_OrderId",
                table: "Product",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Orders_OrderId",
                table: "Product",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
