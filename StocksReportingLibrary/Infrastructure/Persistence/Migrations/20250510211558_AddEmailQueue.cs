using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksReportingLibrary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailQueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailMessages",
                columns: table => new
                {
                    EmailMessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Recipient = table.Column<string>(type: "TEXT", maxLength: 320, nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    RetryCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.EmailMessageId);
                });

            migrationBuilder.CreateTable(
                name: "EmailAttachmentPaths",
                columns: table => new
                {
                    EmailAttachmentPathId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    EmailMessageId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAttachmentPaths", x => x.EmailAttachmentPathId);
                    table.ForeignKey(
                        name: "FK_EmailAttachmentPaths_EmailMessages_EmailMessageId",
                        column: x => x.EmailMessageId,
                        principalTable: "EmailMessages",
                        principalColumn: "EmailMessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailAttachmentPaths_EmailMessageId",
                table: "EmailAttachmentPaths",
                column: "EmailMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailMessages_Status",
                table: "EmailMessages",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAttachmentPaths");

            migrationBuilder.DropTable(
                name: "EmailMessages");
        }
    }
}
