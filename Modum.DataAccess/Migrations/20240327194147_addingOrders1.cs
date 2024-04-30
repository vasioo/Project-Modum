using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modum.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingOrders1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("0b831f59-6d27-4341-90b5-715147c159eb"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("33e0742f-d5b8-4293-8c81-d6d6aa09af63"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("532b05c9-0a73-424d-858c-c7ff3ecdcb4e"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("59cfe593-0101-475b-b361-3b9fcc29af89"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeliveryLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeOfDelivery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfOrdering = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PricePaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StripeReceiptURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

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
                    { new Guid("3350ee78-c6c6-4631-b2fa-1c69e01a1fa5"), "Women" },
                    { new Guid("694ff9fd-5432-4df5-be80-41766d7d3ec5"), "Boys" },
                    { new Guid("c8167db1-c49b-47c1-845c-ecc518fac171"), "Girls" },
                    { new Guid("e3c78799-1c51-4e0a-a51f-73aac90123bc"), "Men" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_OrderId",
                table: "Product",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_OrderId",
                table: "OrderStatus",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Orders_OrderId",
                table: "Product",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Orders_OrderId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Product_OrderId",
                table: "Product");

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("3350ee78-c6c6-4631-b2fa-1c69e01a1fa5"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("694ff9fd-5432-4df5-be80-41766d7d3ec5"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("c8167db1-c49b-47c1-845c-ecc518fac171"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("e3c78799-1c51-4e0a-a51f-73aac90123bc"));

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Product");

            migrationBuilder.InsertData(
                table: "MainCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0b831f59-6d27-4341-90b5-715147c159eb"), "Women" },
                    { new Guid("33e0742f-d5b8-4293-8c81-d6d6aa09af63"), "Girls" },
                    { new Guid("532b05c9-0a73-424d-858c-c7ff3ecdcb4e"), "Boys" },
                    { new Guid("59cfe593-0101-475b-b361-3b9fcc29af89"), "Men" }
                });
        }
    }
}
