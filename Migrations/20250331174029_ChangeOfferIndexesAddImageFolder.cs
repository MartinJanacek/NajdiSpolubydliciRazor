using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NajdiSpolubydliciRazor.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOfferIndexesAddImageFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Offers_Active_CreationDate_District_RentalPrice_MoveInDate",
                table: "Offers");

            migrationBuilder.AddColumn<Guid>(
                name: "ImagesFolder",
                table: "Offers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Active",
                table: "Offers",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_CreationDate",
                table: "Offers",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_District",
                table: "Offers",
                column: "District");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesFolder",
                table: "Offers",
                column: "ImagesFolder");

            migrationBuilder.CreateIndex(
                name: "IX_MoveInDate",
                table: "Offers",
                column: "MoveInDate");

            migrationBuilder.CreateIndex(
                name: "IX_RentalPrice",
                table: "Offers",
                column: "RentalPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Active",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_CreationDate",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_District",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_ImagesFolder",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_MoveInDate",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_RentalPrice",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ImagesFolder",
                table: "Offers");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_Active_CreationDate_District_RentalPrice_MoveInDate",
                table: "Offers",
                columns: new[] { "Active", "CreationDate", "District", "RentalPrice", "MoveInDate" });
        }
    }
}
