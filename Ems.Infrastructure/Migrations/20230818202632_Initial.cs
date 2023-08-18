using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "main");

            migrationBuilder.CreateTable(
                name: "accounts",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    password_salt = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    confirmation_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    confirmation_token = table.Column<string>(type: "text", nullable: true),
                    password_reset_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    password_reset_token = table.Column<string>(type: "text", nullable: true),
                    locked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failed_attempts = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "class_periods",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    starting_at = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ending_at = table.Column<TimeSpan>(type: "interval", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_periods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "class_versions",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "classrooms",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classrooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipient = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    password_reset_token = table.Column<string>(type: "text", nullable: true),
                    password_reset_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    confirmation_token = table.Column<string>(type: "text", nullable: true),
                    confirmation_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lessons",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lessons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_quarter = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "account_roles",
                schema: "main",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_roles", x => new { x.account_id, x.role });
                    table.ForeignKey(
                        name: "FK_account_roles_accounts_account_id",
                        column: x => x.account_id,
                        principalSchema: "main",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "external_accounts",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_email = table.Column<string>(type: "text", nullable: false),
                    external_account_provider = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_external_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_external_accounts_accounts_account_id",
                        column: x => x.account_id,
                        principalSchema: "main",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lecturers",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    middle_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lecturers", x => x.id);
                    table.ForeignKey(
                        name: "FK_lecturers_accounts_account_id",
                        column: x => x.account_id,
                        principalSchema: "main",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_token_id = table.Column<Guid>(type: "uuid", nullable: true),
                    value = table.Column<string>(type: "text", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    used_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_accounts_account_id",
                        column: x => x.account_id,
                        principalSchema: "main",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_refresh_tokens_session_token_id",
                        column: x => x.session_token_id,
                        principalSchema: "main",
                        principalTable: "refresh_tokens",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "idle_periods",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    starting_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ending_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_idle_periods", x => x.id);
                    table.ForeignKey(
                        name: "FK_idle_periods_groups_group_id",
                        column: x => x.group_id,
                        principalSchema: "main",
                        principalTable: "groups",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "students",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    middle_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.id);
                    table.ForeignKey(
                        name: "FK_students_accounts_account_id",
                        column: x => x.account_id,
                        principalSchema: "main",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_students_groups_group_id",
                        column: x => x.group_id,
                        principalSchema: "main",
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_version_id = table.Column<Guid>(type: "uuid", nullable: true),
                    template_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quarter = table.Column<int>(type: "integer", nullable: true),
                    day = table.Column<int>(type: "integer", nullable: true),
                    group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    class_period_id = table.Column<Guid>(type: "uuid", nullable: true),
                    lecturer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    classroom_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    link = table.Column<string>(type: "text", nullable: true),
                    starting_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ending_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id);
                    table.ForeignKey(
                        name: "FK_classes_class_periods_class_period_id",
                        column: x => x.class_period_id,
                        principalSchema: "main",
                        principalTable: "class_periods",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_classes_class_versions_class_version_id",
                        column: x => x.class_version_id,
                        principalSchema: "main",
                        principalTable: "class_versions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_classes_classes_template_id",
                        column: x => x.template_id,
                        principalSchema: "main",
                        principalTable: "classes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_classes_classrooms_classroom_id",
                        column: x => x.classroom_id,
                        principalSchema: "main",
                        principalTable: "classrooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classes_groups_group_id",
                        column: x => x.group_id,
                        principalSchema: "main",
                        principalTable: "groups",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_classes_lecturers_lecturer_id",
                        column: x => x.lecturer_id,
                        principalSchema: "main",
                        principalTable: "lecturers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classes_lessons_lesson_id",
                        column: x => x.lesson_id,
                        principalSchema: "main",
                        principalTable: "lessons",
                        principalColumn: "id",
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
                    type = table.Column<int>(type: "integer", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "student_records",
                schema: "main",
                columns: table => new
                {
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: true),
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_records", x => new { x.student_id, x.class_id });
                    table.ForeignKey(
                        name: "FK_student_records_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "main",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_records_student_record_sessions_session_id",
                        column: x => x.session_id,
                        principalSchema: "main",
                        principalTable: "student_record_sessions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_student_records_students_student_id",
                        column: x => x.student_id,
                        principalSchema: "main",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_classes_class_period_id",
                schema: "main",
                table: "classes",
                column: "class_period_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_class_version_id",
                schema: "main",
                table: "classes",
                column: "class_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_classroom_id",
                schema: "main",
                table: "classes",
                column: "classroom_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_group_id",
                schema: "main",
                table: "classes",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_lecturer_id",
                schema: "main",
                table: "classes",
                column: "lecturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_lesson_id",
                schema: "main",
                table: "classes",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_template_id",
                schema: "main",
                table: "classes",
                column: "template_id");

            migrationBuilder.CreateIndex(
                name: "IX_external_accounts_account_id",
                schema: "main",
                table: "external_accounts",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_idle_periods_group_id",
                schema: "main",
                table: "idle_periods",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_lecturers_account_id",
                schema: "main",
                table: "lecturers",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_account_id",
                schema: "main",
                table: "refresh_tokens",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_session_token_id",
                schema: "main",
                table: "refresh_tokens",
                column: "session_token_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_record_sessions_class_id",
                schema: "main",
                table: "student_record_sessions",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_records_class_id",
                schema: "main",
                table: "student_records",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_records_session_id",
                schema: "main",
                table: "student_records",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_account_id",
                schema: "main",
                table: "students",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_group_id",
                schema: "main",
                table: "students",
                column: "group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_roles",
                schema: "main");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "external_accounts",
                schema: "main");

            migrationBuilder.DropTable(
                name: "idle_periods",
                schema: "main");

            migrationBuilder.DropTable(
                name: "refresh_tokens",
                schema: "main");

            migrationBuilder.DropTable(
                name: "settings",
                schema: "main");

            migrationBuilder.DropTable(
                name: "student_records",
                schema: "main");

            migrationBuilder.DropTable(
                name: "student_record_sessions",
                schema: "main");

            migrationBuilder.DropTable(
                name: "students",
                schema: "main");

            migrationBuilder.DropTable(
                name: "classes",
                schema: "main");

            migrationBuilder.DropTable(
                name: "class_periods",
                schema: "main");

            migrationBuilder.DropTable(
                name: "class_versions",
                schema: "main");

            migrationBuilder.DropTable(
                name: "classrooms",
                schema: "main");

            migrationBuilder.DropTable(
                name: "groups",
                schema: "main");

            migrationBuilder.DropTable(
                name: "lecturers",
                schema: "main");

            migrationBuilder.DropTable(
                name: "lessons",
                schema: "main");

            migrationBuilder.DropTable(
                name: "accounts",
                schema: "main");
        }
    }
}
