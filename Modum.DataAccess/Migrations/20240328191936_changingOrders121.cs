using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modum.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changingOrders121 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "MainCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("05c6fc45-1709-4ddf-9310-891a6b2586c5"), "Men" },
                    { new Guid("72e75ad7-9bd9-4676-b36d-b4d13fda5576"), "Women" },
                    { new Guid("90afb89d-4cfb-431b-80cf-08dd22573c74"), "Girls" },
                    { new Guid("dbd3c69a-3567-4994-9189-d106e2270d33"), "Boys" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("05c6fc45-1709-4ddf-9310-891a6b2586c5"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("72e75ad7-9bd9-4676-b36d-b4d13fda5576"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("90afb89d-4cfb-431b-80cf-08dd22573c74"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("dbd3c69a-3567-4994-9189-d106e2270d33"));

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
    }
}
