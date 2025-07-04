using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.BookAuthoringContext
{
    /// <inheritdoc />
    public partial class CleanDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Books_BooksId",
                schema: "BookAuthoring",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Genres_GenresId",
                schema: "BookAuthoring",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Books_BookId",
                schema: "BookAuthoring",
                table: "Chapters");

            migrationBuilder.RenameColumn(
                name: "GenresId",
                schema: "BookAuthoring",
                table: "BookGenres",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "BooksId",
                schema: "BookAuthoring",
                table: "BookGenres",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenres_GenresId",
                schema: "BookAuthoring",
                table: "BookGenres",
                newName: "IX_BookGenres_GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_BookId",
                schema: "BookAuthoring",
                table: "BookGenres",
                column: "BookId",
                principalSchema: "BookAuthoring",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Genres_GenreId",
                schema: "BookAuthoring",
                table: "BookGenres",
                column: "GenreId",
                principalSchema: "BookAuthoring",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Books_BookId",
                schema: "BookAuthoring",
                table: "Chapters",
                column: "BookId",
                principalSchema: "BookAuthoring",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Books_BookId",
                schema: "BookAuthoring",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Genres_GenreId",
                schema: "BookAuthoring",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Books_BookId",
                schema: "BookAuthoring",
                table: "Chapters");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                schema: "BookAuthoring",
                table: "BookGenres",
                newName: "GenresId");

            migrationBuilder.RenameColumn(
                name: "BookId",
                schema: "BookAuthoring",
                table: "BookGenres",
                newName: "BooksId");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenres_GenreId",
                schema: "BookAuthoring",
                table: "BookGenres",
                newName: "IX_BookGenres_GenresId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_BooksId",
                schema: "BookAuthoring",
                table: "BookGenres",
                column: "BooksId",
                principalSchema: "BookAuthoring",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Genres_GenresId",
                schema: "BookAuthoring",
                table: "BookGenres",
                column: "GenresId",
                principalSchema: "BookAuthoring",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Books_BookId",
                schema: "BookAuthoring",
                table: "Chapters",
                column: "BookId",
                principalSchema: "BookAuthoring",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
