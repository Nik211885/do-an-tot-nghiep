using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations.BookAuthoringContext
{
    /// <inheritdoc />
    public partial class InitBookAuthoringContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BookAuthoring");

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "BookAuthoring",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AvatarUrl = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdateDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: false),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    Visibility = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PolicyReadBook_Price = table.Column<decimal>(type: "numeric", nullable: true),
                    PolicyReadBook_Policy = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    BookReleaseType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "BookAuthoring",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AvatarUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdateDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookTags",
                schema: "BookAuthoring",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookTags_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "BookAuthoring",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                schema: "BookAuthoring",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "BookAuthoring",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenres",
                schema: "BookAuthoring",
                columns: table => new
                {
                    BooksId = table.Column<Guid>(type: "uuid", nullable: false),
                    GenresId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenres", x => new { x.BooksId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_BookGenres_Books_BooksId",
                        column: x => x.BooksId,
                        principalSchema: "BookAuthoring",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenres_Genres_GenresId",
                        column: x => x.GenresId,
                        principalSchema: "BookAuthoring",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChapterVersions",
                schema: "BookAuthoring",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameVersion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DiffContent = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    DiffTitle = table.Column<string>(type: "text", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterVersions_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalSchema: "BookAuthoring",
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_GenresId",
                schema: "BookAuthoring",
                table: "BookGenres",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Slug",
                schema: "BookAuthoring",
                table: "Books",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookTags_BookId",
                schema: "BookAuthoring",
                table: "BookTags",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_BookId",
                schema: "BookAuthoring",
                table: "Chapters",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_Slug",
                schema: "BookAuthoring",
                table: "Chapters",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChapterVersions_ChapterId",
                schema: "BookAuthoring",
                table: "ChapterVersions",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Slug",
                schema: "BookAuthoring",
                table: "Genres",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookGenres",
                schema: "BookAuthoring");

            migrationBuilder.DropTable(
                name: "BookTags",
                schema: "BookAuthoring");

            migrationBuilder.DropTable(
                name: "ChapterVersions",
                schema: "BookAuthoring");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "BookAuthoring");

            migrationBuilder.DropTable(
                name: "Chapters",
                schema: "BookAuthoring");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "BookAuthoring");
        }
    }
}
