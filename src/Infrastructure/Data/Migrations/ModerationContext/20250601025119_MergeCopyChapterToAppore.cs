using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.ModerationContext
{
    /// <inheritdoc />
    public partial class MergeCopyChapterToAppore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Moderation");

            migrationBuilder.CreateTable(
                name: "BookApprovals",
                schema: "Moderation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ContentHash = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    CopyrightChapter_BookTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CopyrightChapter_ChapterTitle = table.Column<string>(type: "text", nullable: true),
                    CopyrightChapter_ChapterContent = table.Column<string>(type: "text", nullable: true),
                    CopyrightChapter_ChapterContentPlainText = table.Column<string>(type: "text", nullable: true),
                    CopyrightChapter_DigitalSignature_SignatureValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CopyrightChapter_DigitalSignature_SignatureAlgorithm = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CopyrightChapter_DigitalSignature_SigningDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CopyrightChapter_DateTimeCopyright = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CopyrightChapter_IsActive = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookApprovals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalDecisions",
                schema: "Moderation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ModeratorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DecisionDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsAutomated = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookApprovalId = table.Column<Guid>(type: "uuid", nullable: true),
                    BookApprovalsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalDecisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalDecisions_BookApprovals_BookApprovalId",
                        column: x => x.BookApprovalId,
                        principalSchema: "Moderation",
                        principalTable: "BookApprovals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalDecisions_BookApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions",
                column: "BookApprovalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalDecisions",
                schema: "Moderation");

            migrationBuilder.DropTable(
                name: "BookApprovals",
                schema: "Moderation");
        }
    }
}
