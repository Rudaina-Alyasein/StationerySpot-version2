using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stationerySpot.Migrations
{
    /// <inheritdoc />
    public partial class AddLibraryIdOnCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibraryId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_LibraryId",
                table: "Categories",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Libraries_LibraryId",
                table: "Categories",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Libraries_LibraryId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_LibraryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "Categories");
        }
    }
}
