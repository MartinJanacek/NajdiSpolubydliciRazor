using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NajdiSpolubydliciRazor.Migrations
{
    /// <inheritdoc />
    public partial class renamedOfferPasswordToOneTimeCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Offers",
                newName: "OneTimeCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OneTimeCode",
                table: "Offers",
                newName: "Password");
        }
    }
}
