using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRekreacija.Services.Migrations.Rekreacija
{
    /// <inheritdoc />
    public partial class ChangeRatingFromIntToFloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "rating",
                table: "tbl_Review",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "rating",
                table: "tbl_Review",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
