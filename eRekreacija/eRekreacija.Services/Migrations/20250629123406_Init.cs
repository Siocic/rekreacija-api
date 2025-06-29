using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRekreacija.Services.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Holiday",
                schema: "Identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Holiday", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SportCategory",
                schema: "Identity",
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
                name: "User",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isApproved = table.Column<bool>(type: "bit", nullable: true),
                    ProfilePicutre = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ChatMessage",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecipientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_ChatMessage_User_RecipientId",
                        column: x => x.RecipientId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_ChatMessage_User_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Notification",
                schema: "Identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_Notification_User_user_id",
                        column: x => x.user_id,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Objects",
                schema: "Identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    price = table.Column<float>(type: "real", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Objects", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_Objects_User_user_id",
                        column: x => x.user_id,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Appointment",
                schema: "Identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointment_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    number_of_players = table.Column<int>(type: "int", nullable: true),
                    is_approved = table.Column<bool>(type: "bit", nullable: true),
                    object_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Appointment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_Appointment_User_user_id",
                        column: x => x.user_id,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_Appointment_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalSchema: "Identity",
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Favorites",
                schema: "Identity",
                columns: table => new
                {
                    object_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tbl_Objectsid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Favorites", x => new { x.user_id, x.object_id });
                    table.ForeignKey(
                        name: "FK_tbl_Favorites_User_user_id",
                        column: x => x.user_id,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_Favorites_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalSchema: "Identity",
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Favorites_tbl_Objects_tbl_Objectsid",
                        column: x => x.tbl_Objectsid,
                        principalSchema: "Identity",
                        principalTable: "tbl_Objects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tbl_ObjectHoliday",
                schema: "Identity",
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
                        principalSchema: "Identity",
                        principalTable: "tbl_Holiday",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_ObjectHoliday_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalSchema: "Identity",
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ObjectSportCategory",
                schema: "Identity",
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
                        principalSchema: "Identity",
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_ObjectSportCategory_tbl_SportCategory_sport_category_id",
                        column: x => x.sport_category_id,
                        principalSchema: "Identity",
                        principalTable: "tbl_SportCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Review",
                schema: "Identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rating = table.Column<float>(type: "real", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    object_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Review", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_Review_User_user_id",
                        column: x => x.user_id,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_Review_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalSchema: "Identity",
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Payment",
                schema: "Identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<float>(type: "real", nullable: false),
                    paid_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    appointment_id = table.Column<int>(type: "int", nullable: false),
                    object_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_Payment_User_user_id",
                        column: x => x.user_id,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_Payment_tbl_Appointment_appointment_id",
                        column: x => x.appointment_id,
                        principalSchema: "Identity",
                        principalTable: "tbl_Appointment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Payment_tbl_Objects_object_id",
                        column: x => x.object_id,
                        principalSchema: "Identity",
                        principalTable: "tbl_Objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Identity",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Identity",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Appointment_object_id",
                schema: "Identity",
                table: "tbl_Appointment",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Appointment_user_id",
                schema: "Identity",
                table: "tbl_Appointment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ChatMessage_RecipientId",
                schema: "Identity",
                table: "tbl_ChatMessage",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ChatMessage_SenderId",
                schema: "Identity",
                table: "tbl_ChatMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Favorites_object_id",
                schema: "Identity",
                table: "tbl_Favorites",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Favorites_tbl_Objectsid",
                schema: "Identity",
                table: "tbl_Favorites",
                column: "tbl_Objectsid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Notification_user_id",
                schema: "Identity",
                table: "tbl_Notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ObjectHoliday_holiday_id",
                schema: "Identity",
                table: "tbl_ObjectHoliday",
                column: "holiday_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Objects_user_id",
                schema: "Identity",
                table: "tbl_Objects",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ObjectSportCategory_sport_category_id",
                schema: "Identity",
                table: "tbl_ObjectSportCategory",
                column: "sport_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Payment_appointment_id",
                schema: "Identity",
                table: "tbl_Payment",
                column: "appointment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Payment_object_id",
                schema: "Identity",
                table: "tbl_Payment",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Payment_user_id",
                schema: "Identity",
                table: "tbl_Payment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Review_object_id",
                schema: "Identity",
                table: "tbl_Review",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Review_user_id",
                schema: "Identity",
                table: "tbl_Review",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Identity",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Identity",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "Identity",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "Identity",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Identity",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_ChatMessage",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Favorites",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Notification",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_ObjectHoliday",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_ObjectSportCategory",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Payment",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Review",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Holiday",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_SportCategory",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Appointment",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Objects",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Identity");
        }
    }
}
