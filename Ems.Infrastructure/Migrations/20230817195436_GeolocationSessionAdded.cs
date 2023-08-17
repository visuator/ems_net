using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Ems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GeolocationSessionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AddColumn<Guid>(
                name: "session_id",
                schema: "main",
                table: "student_records",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "type",
                schema: "main",
                table: "student_records",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "geolocation_student_records",
                schema: "main",
                columns: table => new
                {
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    location = table.Column<Point>(type: "geometry", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geolocation_student_records", x => new { x.student_id, x.class_id });
                    table.ForeignKey(
                        name: "FK_geolocation_student_records_student_records_student_id_clas~",
                        columns: x => new { x.student_id, x.class_id },
                        principalSchema: "main",
                        principalTable: "student_records",
                        principalColumns: new[] { "student_id", "class_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_record_sessions",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ending_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_record_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_student_record_sessions_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "main",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_record_sessions_student_record_sessions_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "main",
                        principalTable: "student_record_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_records_session_id",
                schema: "main",
                table: "student_records",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_record_sessions_class_id",
                schema: "main",
                table: "student_record_sessions",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_record_sessions_TypeId",
                schema: "main",
                table: "student_record_sessions",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_student_records_student_record_sessions_session_id",
                schema: "main",
                table: "student_records",
                column: "session_id",
                principalSchema: "main",
                principalTable: "student_record_sessions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_records_student_record_sessions_session_id",
                schema: "main",
                table: "student_records");

            migrationBuilder.DropTable(
                name: "geolocation_student_records",
                schema: "main");

            migrationBuilder.DropTable(
                name: "student_record_sessions",
                schema: "main");

            migrationBuilder.DropIndex(
                name: "IX_student_records_session_id",
                schema: "main",
                table: "student_records");

            migrationBuilder.DropColumn(
                name: "session_id",
                schema: "main",
                table: "student_records");

            migrationBuilder.DropColumn(
                name: "type",
                schema: "main",
                table: "student_records");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");
        }
    }
}
