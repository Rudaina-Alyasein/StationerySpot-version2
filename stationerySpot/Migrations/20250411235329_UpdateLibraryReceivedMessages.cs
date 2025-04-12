using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stationerySpot.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLibraryReceivedMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ContactUs__Targe__17F790F9",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "ContactUs");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "ContactUs",
                newName: "LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactUs_TargetId",
                table: "ContactUs",
                newName: "IX_ContactUs_LibraryId");

            migrationBuilder.AddColumn<int>(
                name: "ReceiverId",
                table: "ContactUs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_ReceiverId",
                table: "ContactUs",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactUs_Libraries_LibraryId",
                table: "ContactUs",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__ContactUs__Recei__17F790F9",
                table: "ContactUs",
                column: "ReceiverId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactUs_Libraries_LibraryId",
                table: "ContactUs");

            migrationBuilder.DropForeignKey(
                name: "FK__ContactUs__Recei__17F790F9",
                table: "ContactUs");

            migrationBuilder.DropIndex(
                name: "IX_ContactUs_ReceiverId",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "ContactUs");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "ContactUs",
                newName: "TargetId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactUs_LibraryId",
                table: "ContactUs",
                newName: "IX_ContactUs_TargetId");

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                table: "ContactUs",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK__ContactUs__Targe__17F790F9",
                table: "ContactUs",
                column: "TargetId",
                principalTable: "Libraries",
                principalColumn: "Id");
        }
    }
}
