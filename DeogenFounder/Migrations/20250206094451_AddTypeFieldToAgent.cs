using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeogenFounder.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeFieldToAgent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Agents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Agents");
        }
    }
}
