using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.BookAuthoringContext
{
    /// <inheritdoc />
    public partial class AddedColumnCurrentForChapter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrentVersion",
                schema: "BookAuthoring",
                table: "Chapters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentVersion",
                schema: "BookAuthoring",
                table: "Chapters");
        }
    }
}
