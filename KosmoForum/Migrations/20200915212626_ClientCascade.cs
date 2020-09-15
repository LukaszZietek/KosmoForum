using Microsoft.EntityFrameworkCore.Migrations;

namespace KosmoForum.Migrations
{
    public partial class ClientCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ForumPosts_ForumPostId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Opinions_ForumPosts_ForumPostId",
                table: "Opinions");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ForumPosts_ForumPostId",
                table: "Images",
                column: "ForumPostId",
                principalTable: "ForumPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opinions_ForumPosts_ForumPostId",
                table: "Opinions",
                column: "ForumPostId",
                principalTable: "ForumPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ForumPosts_ForumPostId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Opinions_ForumPosts_ForumPostId",
                table: "Opinions");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ForumPosts_ForumPostId",
                table: "Images",
                column: "ForumPostId",
                principalTable: "ForumPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Opinions_ForumPosts_ForumPostId",
                table: "Opinions",
                column: "ForumPostId",
                principalTable: "ForumPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
