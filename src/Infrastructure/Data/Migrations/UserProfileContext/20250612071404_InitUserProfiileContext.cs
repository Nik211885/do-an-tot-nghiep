using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.UserProfileContext
{
    /// <inheritdoc />
    public partial class InitUserProfiileContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UserProfile");

            migrationBuilder.CreateTable(
                name: "UserProfile",
                schema: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Bio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CountFollowing = table.Column<int>(type: "integer", nullable: false),
                    CoutFollowers = table.Column<int>(type: "integer", nullable: false),
                    CountFavoriteBook = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteBook",
                schema: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FavoriteBookId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteBook", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteBook_UserProfile_UserId",
                        column: x => x.UserId,
                        principalSchema: "UserProfile",
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Follower",
                schema: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowingId = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follower", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follower_UserProfile_FollowerId",
                        column: x => x.FollowerId,
                        principalSchema: "UserProfile",
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Follower_UserProfile_FollowingId",
                        column: x => x.FollowingId,
                        principalSchema: "UserProfile",
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchHistory",
                schema: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SearchTerm = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SearchDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchHistory_UserProfile_UserId",
                        column: x => x.UserId,
                        principalSchema: "UserProfile",
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBook_UserId",
                schema: "UserProfile",
                table: "FavoriteBook",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Follower_FollowerId",
                schema: "UserProfile",
                table: "Follower",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Follower_FollowingId",
                schema: "UserProfile",
                table: "Follower",
                column: "FollowingId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistory_UserId",
                schema: "UserProfile",
                table: "SearchHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteBook",
                schema: "UserProfile");

            migrationBuilder.DropTable(
                name: "Follower",
                schema: "UserProfile");

            migrationBuilder.DropTable(
                name: "SearchHistory",
                schema: "UserProfile");

            migrationBuilder.DropTable(
                name: "UserProfile",
                schema: "UserProfile");
        }
    }
}
