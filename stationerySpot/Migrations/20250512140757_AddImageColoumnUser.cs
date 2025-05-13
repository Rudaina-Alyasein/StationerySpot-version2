using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stationerySpot.Migrations
{
    /// <inheritdoc />
    public partial class AddImageColoumnUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "OfferComments");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "OfferComments");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "OfferComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OfferComments_UserId",
                table: "OfferComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComments_Users_UserId",
                table: "OfferComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferComments_Users_UserId",
                table: "OfferComments");

            migrationBuilder.DropIndex(
                name: "IX_OfferComments_UserId",
                table: "OfferComments");

            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OfferComments");

            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "OfferComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "OfferComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
