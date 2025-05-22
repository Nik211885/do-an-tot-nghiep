using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.BookAuthoringContext
{
    /// <inheritdoc />
    public partial class AddIndentiyandCountBookForGenreActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountBook",
                schema: "BookAuthoring",
                table: "Genres",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CreateUserId",
                schema: "BookAuthoring",
                table: "Genres",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                schema: "BookAuthoring",
                table: "Genres",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Genres_Name",
                schema: "BookAuthoring",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "CountBook",
                schema: "BookAuthoring",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                schema: "BookAuthoring",
                table: "Genres");
        }
    }
}
