using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.ModerationContext
{
    /// <inheritdoc />
    public partial class changeReleaseBookApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalDecisions_BookApprovals_BookApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalDecisions_BookApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions");

            migrationBuilder.DropColumn(
                name: "ChapterContent",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "ChapterNumber",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "ChapterSlug",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "ChapterTitle",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "CopyrightChapter_BookTitle",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "CopyrightChapter_ChapterContent",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "CopyrightChapter_ChapterNumber",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "CopyrightChapter_ChapterSlug",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "CopyrightChapter_ChapterTitle",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "CopyrightChapter_DateTimeCopyright",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "CopyrightChapter_DigitalSignature_SignatureAlgorithm",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "CopyrightChapter_DigitalSignature_SignatureValue",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "CopyrightChapter_DigitalSignature_SigningDateTime",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "BookApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions");

            migrationBuilder.DropColumn(
                name: "IsAutomated",
                schema: "Moderation",
                table: "ApprovalDecisions");

            migrationBuilder.RenameColumn(
                name: "BookApprovalsId",
                schema: "Moderation",
                table: "ApprovalDecisions",
                newName: "ChapterApprovalId");

            migrationBuilder.AlterColumn<string>(
                name: "BookTitle",
                schema: "Moderation",
                table: "BookApprovals",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Moderation",
                table: "BookApprovals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ChapterApproval",
                schema: "Moderation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookApprovalId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChapterContent = table.Column<string>(type: "text", nullable: false),
                    ChapterTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ChapterNumber = table.Column<int>(type: "integer", nullable: false),
                    ChapterSlug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    CopyrightChapter_ChapterTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CopyrightChapter_ChapterSlug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CopyrightChapter_ChapterNumber = table.Column<int>(type: "integer", nullable: true),
                    CopyrightChapter_ChapterContent = table.Column<string>(type: "text", nullable: true),
                    CopyrightChapter_DateTimeCopyright = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterApproval_BookApprovals_BookApprovalId",
                        column: x => x.BookApprovalId,
                        principalSchema: "Moderation",
                        principalTable: "BookApprovals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalDecisions_ChapterApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions",
                column: "ChapterApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterApproval_BookApprovalId",
                schema: "Moderation",
                table: "ChapterApproval",
                column: "BookApprovalId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalDecisions_ChapterApproval_ChapterApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions",
                column: "ChapterApprovalId",
                principalSchema: "Moderation",
                principalTable: "ChapterApproval",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalDecisions_ChapterApproval_ChapterApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions");

            migrationBuilder.DropTable(
                name: "ChapterApproval",
                schema: "Moderation");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalDecisions_ChapterApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.RenameColumn(
                name: "ChapterApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions",
                newName: "BookApprovalsId");

            migrationBuilder.AlterColumn<string>(
                name: "BookTitle",
                schema: "Moderation",
                table: "BookApprovals",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ChapterContent",
                schema: "Moderation",
                table: "BookApprovals",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ChapterId",
                schema: "Moderation",
                table: "BookApprovals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "ChapterNumber",
                schema: "Moderation",
                table: "BookApprovals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ChapterSlug",
                schema: "Moderation",
                table: "BookApprovals",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChapterTitle",
                schema: "Moderation",
                table: "BookApprovals",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CopyrightChapter_BookTitle",
                schema: "Moderation",
                table: "BookApprovals",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CopyrightChapter_ChapterContent",
                schema: "Moderation",
                table: "BookApprovals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CopyrightChapter_ChapterNumber",
                schema: "Moderation",
                table: "BookApprovals",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CopyrightChapter_ChapterSlug",
                schema: "Moderation",
                table: "BookApprovals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CopyrightChapter_ChapterTitle",
                schema: "Moderation",
                table: "BookApprovals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CopyrightChapter_DateTimeCopyright",
                schema: "Moderation",
                table: "BookApprovals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CopyrightChapter_DigitalSignature_SignatureAlgorithm",
                schema: "Moderation",
                table: "BookApprovals",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CopyrightChapter_DigitalSignature_SignatureValue",
                schema: "Moderation",
                table: "BookApprovals",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CopyrightChapter_DigitalSignature_SigningDateTime",
                schema: "Moderation",
                table: "BookApprovals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "Moderation",
                table: "BookApprovals",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SubmittedAt",
                schema: "Moderation",
                table: "BookApprovals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "Moderation",
                table: "BookApprovals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BookApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAutomated",
                schema: "Moderation",
                table: "ApprovalDecisions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalDecisions_BookApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions",
                column: "BookApprovalId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalDecisions_BookApprovals_BookApprovalId",
                schema: "Moderation",
                table: "ApprovalDecisions",
                column: "BookApprovalId",
                principalSchema: "Moderation",
                principalTable: "BookApprovals",
                principalColumn: "Id");
        }
    }
}
