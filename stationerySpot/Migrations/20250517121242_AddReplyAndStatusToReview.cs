using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stationerySpot.Migrations
{
    /// <inheritdoc />
    public partial class AddReplyAndStatusToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reply",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reply",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reviews");
        }
    }
}
