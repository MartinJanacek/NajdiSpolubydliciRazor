using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NajdiSpolubydliciRazor.Migrations
{
    /// <inheritdoc />
    public partial class ChangeandAddUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Demands_Email",
                table: "Demands");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_Email",
                table: "Offers",
                newName: "UX_Email");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Demands",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "UX_ImagesDirectoryId",
                table: "Offers",
                column: "ImagesDirectoryId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_ImagesDirectoryId",
                table: "Offers");

            migrationBuilder.RenameIndex(
                name: "UX_Email",
                table: "Offers",
                newName: "IX_Offers_Email");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Demands",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_Email",
                table: "Demands",
                column: "Email",
                unique: true);
        }
    }
}
