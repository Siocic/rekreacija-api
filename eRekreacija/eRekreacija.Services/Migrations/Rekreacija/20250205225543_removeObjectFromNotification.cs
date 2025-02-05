using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRekreacija.Services.Migrations.Rekreacija
{
    /// <inheritdoc />
    public partial class removeObjectFromNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Notification_tbl_Objects_object_id",
                table: "tbl_Notification");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Notification_object_id",
                table: "tbl_Notification");

            migrationBuilder.DropColumn(
                name: "object_id",
                table: "tbl_Notification");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "object_id",
                table: "tbl_Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Notification_object_id",
                table: "tbl_Notification",
                column: "object_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Notification_tbl_Objects_object_id",
                table: "tbl_Notification",
                column: "object_id",
                principalTable: "tbl_Objects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
