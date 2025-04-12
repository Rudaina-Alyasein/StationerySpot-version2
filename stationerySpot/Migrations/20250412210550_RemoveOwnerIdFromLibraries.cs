using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stationerySpot.Migrations
{
    public partial class RemoveOwnerIdFromLibraries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // حذف العلاقة ForeignKey المرتبطة بـ OwnerId
            migrationBuilder.DropForeignKey(
                name: "FK__Libraries__Owner__3D5E1FD2",
                table: "Libraries");

            // حذف العمود OwnerId من جدول Libraries
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Libraries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // في حالة التراجع عن الهجرة، نضيف العمود OwnerId من جديد
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Libraries",
                nullable: true);

            // إعادة إضافة العلاقة ForeignKey بين العمود OwnerId وجدول Users
            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_Users_OwnerId",
                table: "Libraries",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
