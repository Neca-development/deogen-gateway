using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeogenFounder.Migrations
{
    /// <inheritdoc />
    public partial class AddApiUrlApiKeyToAgent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "Agents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApiUrl",
                table: "Agents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "ApiUrl",
                table: "Agents");
        }
    }
}
