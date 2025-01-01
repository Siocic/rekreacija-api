using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRekreacija.Services.Migrations.Rekreacija
{
    /// <inheritdoc />
    public partial class initRekreacija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Holiday",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    end_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Holiday", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Objects",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    updated_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<float>(type: "real", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Objects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SportCategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SportCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Appointment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointment_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    start_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    end_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    is_approved = table.Column<bool>(type: "bit", nullable: true),
                    object_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Appointment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_Appointment_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Favorites",
                columns: table => new
                {
                    object_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Favorites", x => new { x.user_id, x.object_id });
                    table.ForeignKey(
                        name: "FK_tbl_Favorites_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Notification",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    object_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_Notification_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ObjectHoliday",
                columns: table => new
                {
                    object_id = table.Column<int>(type: "int", nullable: false),
                    holiday_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ObjectHoliday", x => new { x.object_id, x.holiday_id });
                    table.ForeignKey(
                        name: "FK_tbl_ObjectHoliday_tbl_Holiday_holiday_id",
                        column: x => x.holiday_id,
                        principalTable: "tbl_Holiday",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_ObjectHoliday_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Review",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    object_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Review", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_Review_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ObjectSportCategory",
                columns: table => new
                {
                    object_id = table.Column<int>(type: "int", nullable: false),
                    sport_category_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ObjectSportCategory", x => new { x.object_id, x.sport_category_id });
                    table.ForeignKey(
                        name: "FK_tbl_ObjectSportCategory_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_ObjectSportCategory_tbl_SportCategory_sport_category_id",
                        column: x => x.sport_category_id,
                        principalTable: "tbl_SportCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Payment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<float>(type: "real", nullable: false),
                    paid_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    appointment_id = table.Column<int>(type: "int", nullable: false),
                    object_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_Payment_tbl_Appointment_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "tbl_Appointment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Payment_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Appointment_object_id",
                table: "tbl_Appointment",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Favorites_object_id",
                table: "tbl_Favorites",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Notification_object_id",
                table: "tbl_Notification",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ObjectHoliday_holiday_id",
                table: "tbl_ObjectHoliday",
                column: "holiday_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ObjectSportCategory_sport_category_id",
                table: "tbl_ObjectSportCategory",
                column: "sport_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Payment_appointment_id",
                table: "tbl_Payment",
                column: "appointment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Payment_object_id",
                table: "tbl_Payment",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Review_object_id",
                table: "tbl_Review",
                column: "object_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Favorites");

            migrationBuilder.DropTable(
                name: "tbl_Notification");

            migrationBuilder.DropTable(
                name: "tbl_ObjectHoliday");

            migrationBuilder.DropTable(
                name: "tbl_ObjectSportCategory");

            migrationBuilder.DropTable(
                name: "tbl_Payment");

            migrationBuilder.DropTable(
                name: "tbl_Review");

            migrationBuilder.DropTable(
                name: "tbl_Holiday");

            migrationBuilder.DropTable(
                name: "tbl_SportCategory");

            migrationBuilder.DropTable(
                name: "tbl_Appointment");

            migrationBuilder.DropTable(
                name: "tbl_Objects");
        }
    }
}
