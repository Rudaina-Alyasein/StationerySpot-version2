using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stationerySpot.Migrations
{
    /// <inheritdoc />
    public partial class AddLibraryNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Libraries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Libraries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteURL",
                table: "Libraries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkingHours",
                table: "Libraries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "WebsiteURL",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "Libraries");
        }
    }
}
