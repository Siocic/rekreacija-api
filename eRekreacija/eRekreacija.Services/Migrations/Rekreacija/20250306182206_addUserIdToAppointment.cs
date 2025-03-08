using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRekreacija.Services.Migrations.Rekreacija
{
    /// <inheritdoc />
    public partial class addUserIdToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "tbl_Appointment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_id",
                table: "tbl_Appointment");
        }
    }
}
