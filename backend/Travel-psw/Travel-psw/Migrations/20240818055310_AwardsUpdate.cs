using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_psw.Migrations
{
    /// <inheritdoc />
    public partial class AwardsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAwarded",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAwarded",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Users");
        }
    }
}
