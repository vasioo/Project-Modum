using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modum.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changingWorkerEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_AspNetUsers_UserId",
                table: "Workers");

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("bdd6ce71-65c4-4738-ab94-2256818838d5"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("da49968c-c780-4921-806f-e767f104e223"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("f2c649f6-8eb6-4a93-80d2-d46e8c6db8d7"));

            migrationBuilder.DeleteData(
                table: "MainCategory",
                keyColumn: "Id",
                keyValue: new Guid("f3792f8f-0bfa-4b32-88fa-d2df42e1f619"));

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Workers",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Workers_UserId",
                table: "Workers",
                newName: "IX_Workers_AppUserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_AspNetUsers_AppUserId",
                table: "Workers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_AspNetUsers_AppUserId",
                table: "Workers");

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

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Workers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Workers_AppUserId",
                table: "Workers",
                newName: "IX_Workers_UserId");

            migrationBuilder.InsertData(
                table: "MainCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("bdd6ce71-65c4-4738-ab94-2256818838d5"), "Women" },
                    { new Guid("da49968c-c780-4921-806f-e767f104e223"), "Men" },
                    { new Guid("f2c649f6-8eb6-4a93-80d2-d46e8c6db8d7"), "Girls" },
                    { new Guid("f3792f8f-0bfa-4b32-88fa-d2df42e1f619"), "Boys" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_AspNetUsers_UserId",
                table: "Workers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
