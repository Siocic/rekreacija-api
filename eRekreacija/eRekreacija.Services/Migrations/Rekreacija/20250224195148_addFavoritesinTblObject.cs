using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRekreacija.Services.Migrations.Rekreacija
{
    /// <inheritdoc />
    public partial class addFavoritesinTblObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tbl_Objectsid",
                table: "tbl_Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Favorites_tbl_Objectsid",
                table: "tbl_Favorites",
                column: "tbl_Objectsid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Favorites_tbl_Objects_tbl_Objectsid",
                table: "tbl_Favorites",
                column: "tbl_Objectsid",
                principalTable: "tbl_Objects",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Favorites_tbl_Objects_tbl_Objectsid",
                table: "tbl_Favorites");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Favorites_tbl_Objectsid",
                table: "tbl_Favorites");

            migrationBuilder.DropColumn(
                name: "tbl_Objectsid",
                table: "tbl_Favorites");
        }
    }
}
