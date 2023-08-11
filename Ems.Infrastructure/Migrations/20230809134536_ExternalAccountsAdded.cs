using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExternalAccountsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "external_accounts",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_account_provider = table.Column<int>(type: "integer", nullable: false),
                    external_email = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_external_accounts_account_id",
                schema: "main",
                table: "external_accounts",
                column: "account_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "external_accounts",
                schema: "main");
        }
    }
}
