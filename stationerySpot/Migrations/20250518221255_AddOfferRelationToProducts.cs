using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stationerySpot.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferRelationToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_OfferId",
                table: "Products",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK__Products__OfferId",
                table: "Products",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Products__OfferId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OfferId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Products");
        }
    }
}
