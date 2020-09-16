using Microsoft.EntityFrameworkCore.Migrations;

namespace KosmoForum.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opinions_Users_UserId",
                table: "Opinions");

            migrationBuilder.AddForeignKey(
                name: "FK_Opinions_Users_UserId",
                table: "Opinions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opinions_Users_UserId",
                table: "Opinions");

            migrationBuilder.AddForeignKey(
                name: "FK_Opinions_Users_UserId",
                table: "Opinions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
