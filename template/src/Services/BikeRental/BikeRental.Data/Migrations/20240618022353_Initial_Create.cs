using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BikeRental.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bike_rental");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "bike_rental",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "bike_rental",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bikes",
                schema: "bike_rental",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Plate = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bikes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "delivery_request_notifications",
                schema: "bike_rental",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DeliveryRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryRiderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_request_notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "delivery_riders",
                schema: "bike_rental",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Cnpj_Value = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    Cnh_Type = table.Column<int>(type: "integer", nullable: true),
                    Cnh_Number = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    Cnh_Image = table.Column<string>(type: "text", nullable: true),
                    CurrentBikeId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentDeliveryRequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Birthday = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_riders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "bike_rental",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_role_id",
                        column: x => x.role_id,
                        principalSchema: "bike_rental",
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "bike_rental",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalSchema: "bike_rental",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "bike_rental",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalSchema: "bike_rental",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "bike_rental",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_role_id",
                        column: x => x.role_id,
                        principalSchema: "bike_rental",
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalSchema: "bike_rental",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "bike_rental",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalSchema: "bike_rental",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "delivery_request",
                schema: "bike_rental",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryRiderId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PriceCents = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_delivery_request_delivery_riders_DeliveryRiderId",
                        column: x => x.DeliveryRiderId,
                        principalSchema: "bike_rental",
                        principalTable: "delivery_riders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "rental",
                schema: "bike_rental",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BikeId = table.Column<long>(type: "bigint", nullable: false),
                    DeliveryRiderId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpectedReturnAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DailyPriceCents = table.Column<int>(type: "integer", nullable: false),
                    PriceCents = table.Column<int>(type: "integer", nullable: false),
                    PenaltyPriceCents = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rental_bikes_BikeId",
                        column: x => x.BikeId,
                        principalSchema: "bike_rental",
                        principalTable: "bikes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rental_delivery_riders_DeliveryRiderId",
                        column: x => x.DeliveryRiderId,
                        principalSchema: "bike_rental",
                        principalTable: "delivery_riders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_role_id",
                schema: "bike_rental",
                table: "AspNetRoleClaims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "bike_rental",
                table: "AspNetRoles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_user_id",
                schema: "bike_rental",
                table: "AspNetUserClaims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_user_id",
                schema: "bike_rental",
                table: "AspNetUserLogins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_role_id",
                schema: "bike_rental",
                table: "AspNetUserRoles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "bike_rental",
                table: "AspNetUsers",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "bike_rental",
                table: "AspNetUsers",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bikes_CreatedAt",
                schema: "bike_rental",
                table: "bikes",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_bikes_DeletedAt",
                schema: "bike_rental",
                table: "bikes",
                column: "DeletedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_bikes_Id",
                schema: "bike_rental",
                table: "bikes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_bikes_Plate",
                schema: "bike_rental",
                table: "bikes",
                column: "Plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bikes_UpdatedAt",
                schema: "bike_rental",
                table: "bikes",
                column: "UpdatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_request_DeliveryRiderId",
                schema: "bike_rental",
                table: "delivery_request",
                column: "DeliveryRiderId");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_request_Id",
                schema: "bike_rental",
                table: "delivery_request",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_request_Status",
                schema: "bike_rental",
                table: "delivery_request",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_request_notifications_DeliveryRiderId_DeliveryRequ~",
                schema: "bike_rental",
                table: "delivery_request_notifications",
                columns: new[] { "DeliveryRiderId", "DeliveryRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_request_notifications_Id",
                schema: "bike_rental",
                table: "delivery_request_notifications",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_riders_Cnh_Number",
                schema: "bike_rental",
                table: "delivery_riders",
                column: "Cnh_Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_riders_Cnpj_Value",
                schema: "bike_rental",
                table: "delivery_riders",
                column: "Cnpj_Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_riders_CreatedAt",
                schema: "bike_rental",
                table: "delivery_riders",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_riders_DeletedAt",
                schema: "bike_rental",
                table: "delivery_riders",
                column: "DeletedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_riders_Id",
                schema: "bike_rental",
                table: "delivery_riders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_riders_UpdatedAt",
                schema: "bike_rental",
                table: "delivery_riders",
                column: "UpdatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_riders_UserId",
                schema: "bike_rental",
                table: "delivery_riders",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rental_BikeId",
                schema: "bike_rental",
                table: "rental",
                column: "BikeId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_DeliveryRiderId",
                schema: "bike_rental",
                table: "rental",
                column: "DeliveryRiderId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_Id",
                schema: "bike_rental",
                table: "rental",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_rental_StartAt_EndAt",
                schema: "bike_rental",
                table: "rental",
                columns: new[] { "StartAt", "EndAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "delivery_request",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "delivery_request_notifications",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "rental",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "bikes",
                schema: "bike_rental");

            migrationBuilder.DropTable(
                name: "delivery_riders",
                schema: "bike_rental");
        }
    }
}
