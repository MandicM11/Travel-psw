using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_psw.Migrations
{
    /// <inheritdoc />
    public partial class AuthorProblems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Problems_TourId",
                table: "Problems",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Tours_TourId",
                table: "Problems",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Tours_TourId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_TourId",
                table: "Problems");
        }
    }
}
