using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeogenFounder.Migrations
{
    /// <inheritdoc />
    public partial class AddToHumanField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ToHuman",
                table: "Messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToHuman",
                table: "Messages");
        }
    }
}
