using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Authorization.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5a9b60b9-05a0-4c44-904c-f3d5274a41cb"), null, "User", "USER" },
                    { new Guid("de4c8043-9e24-4ba2-a397-939fcd307c35"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDay", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Snils", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a"), 0, new DateOnly(2001, 6, 6), "80ee415f-405c-4bbb-8d72-251b9c0d37c1", new DateTime(2023, 12, 4, 15, 8, 33, 95, DateTimeKind.Utc).AddTicks(1413), "icestormyy-user@mail.ru", false, "Mikhail", 1, "Tolmachev", false, null, "Evgenievich", null, "ICESTORMY-USER", "AQAAAAIAAYagAAAAEKE/pdEICYAT9QuvIIo8rHAQE3cgNN5hW7JvaVnUQW8sYlzy70H1LlxOoC1xUmc59A==", "89094316687", false, null, null, false, null, "IceStormy-user" },
                    { new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6"), 0, new DateOnly(2001, 6, 6), "f3e4a882-3f74-402a-86a0-94dde12b4bc5", new DateTime(2023, 12, 4, 15, 8, 33, 95, DateTimeKind.Utc).AddTicks(1289), "icestormyy-admin@mail.ru", false, "Mikhail", 1, "Tolmachev", false, null, "Evgenievich", null, "ICESTORMY-ADMIN", "AQAAAAIAAYagAAAAEKE/pdEICYAT9QuvIIo8rHAQE3cgNN5hW7JvaVnUQW8sYlzy70H1LlxOoC1xUmc59A==", "81094316687", false, null, null, false, null, "IceStormy-admin" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("5a9b60b9-05a0-4c44-904c-f3d5274a41cb"), new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a") },
                    { new Guid("de4c8043-9e24-4ba2-a397-939fcd307c35"), new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("5a9b60b9-05a0-4c44-904c-f3d5274a41cb"), new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("de4c8043-9e24-4ba2-a397-939fcd307c35"), new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5a9b60b9-05a0-4c44-904c-f3d5274a41cb"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("de4c8043-9e24-4ba2-a397-939fcd307c35"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e1f83d38-56a7-435b-94bd-fe891ed0f03a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f2343d16-e610-4a73-a0f0-b9f63df511e6"));
        }
    }
}
