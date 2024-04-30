using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modum.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("360a9039-f479-499d-ac08-6adf7dcc15ee"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("998eec9e-3fa3-49a6-bee9-d3bb7308acd0"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("fa24b471-4b1f-4238-a12b-420786e02496"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("fb672249-6922-420c-864b-b6b1a88bddc9"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "MainCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("360a9039-f479-499d-ac08-6adf7dcc15ee"), "Boys" },
                    { new Guid("998eec9e-3fa3-49a6-bee9-d3bb7308acd0"), "Women" },
                    { new Guid("fa24b471-4b1f-4238-a12b-420786e02496"), "Men" },
                    { new Guid("fb672249-6922-420c-864b-b6b1a88bddc9"), "Girls" }
                });
        }
    }
}
