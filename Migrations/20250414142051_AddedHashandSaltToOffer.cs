using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NajdiSpolubydliciRazor.Migrations
{
    /// <inheritdoc />
    public partial class AddedHashandSaltToOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImagesFolder",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ImagesFolder",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "OneTimeCode",
                table: "Offers");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "Offers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Offers");

            migrationBuilder.AddColumn<Guid>(
                name: "ImagesFolder",
                table: "Offers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "OneTimeCode",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImagesFolder",
                table: "Offers",
                column: "ImagesFolder");
        }
    }
}
