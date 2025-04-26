using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stationerySpot.Migrations
{
    /// <inheritdoc />
    public partial class UpdatecontactUs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ContactUs__Recei__17F790F9",
                table: "ContactUs");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                table: "ContactUs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ContactUs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__ContactUs__Recei__17F790F9",
                table: "ContactUs",
                column: "ReceiverId",
                principalTable: "Libraries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ContactUs__Recei__17F790F9",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ContactUs");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                table: "ContactUs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__ContactUs__Recei__17F790F9",
                table: "ContactUs",
                column: "ReceiverId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
