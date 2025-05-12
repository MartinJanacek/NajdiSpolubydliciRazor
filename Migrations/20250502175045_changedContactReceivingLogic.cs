using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NajdiSpolubydliciRazor.Migrations
{
    /// <inheritdoc />
    public partial class changedContactReceivingLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactReceivers",
                table: "ContactReceivers");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "ContactReceivers");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "ContactReceivers",
                newName: "UrlOpened");

            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "ContactReceivers",
                newName: "Code");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ContactReceivers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ContactReceivers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "AdvertismentId",
                table: "ContactReceivers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TypeOfAdvertising",
                table: "ContactReceivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactReceivers",
                table: "ContactReceivers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactReceivers",
                table: "ContactReceivers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ContactReceivers");

            migrationBuilder.DropColumn(
                name: "AdvertismentId",
                table: "ContactReceivers");

            migrationBuilder.DropColumn(
                name: "TypeOfAdvertising",
                table: "ContactReceivers");

            migrationBuilder.RenameColumn(
                name: "UrlOpened",
                table: "ContactReceivers",
                newName: "IsVerified");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "ContactReceivers",
                newName: "Hash");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ContactReceivers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "ContactReceivers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactReceivers",
                table: "ContactReceivers",
                column: "Email");
        }
    }
}
