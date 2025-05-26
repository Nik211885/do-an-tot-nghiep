using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.BookAuthoringContext
{
    /// <inheritdoc />
    public partial class RemoveColumnVersionNumberInBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VersionNumber",
                schema: "BookAuthoring",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "NameVersion",
                schema: "BookAuthoring",
                table: "ChapterVersions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "BookAuthoring",
                table: "Chapters",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameVersion",
                schema: "BookAuthoring",
                table: "ChapterVersions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "BookAuthoring",
                table: "Chapters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "VersionNumber",
                schema: "BookAuthoring",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
