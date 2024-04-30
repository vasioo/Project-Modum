using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modum.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingStripeConnectivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "StripeId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "MainCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7d37a63f-a340-4a67-8d4b-308c9de0b1e9"), "Men" },
                    { new Guid("8e2313f4-4434-47f4-846c-c14b0aacaa2b"), "Women" },
                    { new Guid("a3c16063-9af9-4ef9-9027-5ea2106f42d9"), "Boys" },
                    { new Guid("defba104-e633-4011-8d11-f59045b7993c"), "Girls" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("7d37a63f-a340-4a67-8d4b-308c9de0b1e9"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("8e2313f4-4434-47f4-846c-c14b0aacaa2b"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("a3c16063-9af9-4ef9-9027-5ea2106f42d9"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("defba104-e633-4011-8d11-f59045b7993c"));

            migrationBuilder.DropColumn(
                name: "StripeId",
                table: "Orders");

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
    }
}
