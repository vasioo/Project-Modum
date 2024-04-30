using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modum.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingOrders11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
