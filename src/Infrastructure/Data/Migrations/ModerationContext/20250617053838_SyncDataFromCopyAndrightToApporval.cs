using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.ModerationContext
{
    /// <inheritdoc />
    public partial class SyncDataFromCopyAndrightToApporval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CopyrightChapter_IsActive",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.RenameColumn(
                name: "CopyrightChapter_ChapterContentPlainText",
                schema: "Moderation",
                table: "BookApprovals",
                newName: "CopyrightChapter_ChapterSlug");

            migrationBuilder.RenameColumn(
                name: "ContentHash",
                schema: "Moderation",
                table: "BookApprovals",
                newName: "ChapterTitle");

            migrationBuilder.AddColumn<string>(
                name: "BookTitle",
                schema: "Moderation",
                table: "BookApprovals",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChapterContent",
                schema: "Moderation",
                table: "BookApprovals",
                type: "text",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<int>(
                name: "CopyrightChapter_ChapterNumber",
                schema: "Moderation",
                table: "BookApprovals",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookTitle",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.DropColumn(
                name: "ChapterContent",
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
                name: "CopyrightChapter_ChapterNumber",
                schema: "Moderation",
                table: "BookApprovals");

            migrationBuilder.RenameColumn(
                name: "CopyrightChapter_ChapterSlug",
                schema: "Moderation",
                table: "BookApprovals",
                newName: "CopyrightChapter_ChapterContentPlainText");

            migrationBuilder.RenameColumn(
                name: "ChapterTitle",
                schema: "Moderation",
                table: "BookApprovals",
                newName: "ContentHash");

            migrationBuilder.AddColumn<bool>(
                name: "CopyrightChapter_IsActive",
                schema: "Moderation",
                table: "BookApprovals",
                type: "boolean",
                nullable: true);
        }
    }
}
