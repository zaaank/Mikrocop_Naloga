using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRepo.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "ApiKey", "CreatedAt", "IsActive", "Name" },
                values: new object[] { new Guid("be054320-302a-430c-9602-535352c713b1"), "be054320-302a-430c-9602-535352c713b1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "DefaultClient" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("be054320-302a-430c-9602-535352c713b1"));
        }
    }
}
