using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Task.Server.Migrations
{
    /// <inheritdoc />
    public partial class nolists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseBook_AspNetUsers_DatabaseUserId",
                table: "DatabaseBook");

            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseBook_DatabaseAuthor_DatabaseAuthorId",
                table: "DatabaseBook");

            migrationBuilder.DropIndex(
                name: "IX_DatabaseBook_DatabaseAuthorId",
                table: "DatabaseBook");

            migrationBuilder.DropIndex(
                name: "IX_DatabaseBook_DatabaseUserId",
                table: "DatabaseBook");

            migrationBuilder.DropColumn(
                name: "DatabaseAuthorId",
                table: "DatabaseBook");

            migrationBuilder.DropColumn(
                name: "DatabaseUserId",
                table: "DatabaseBook");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DatabaseAuthorId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabaseUserId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseBook_DatabaseAuthorId",
                table: "DatabaseBook",
                column: "DatabaseAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseBook_DatabaseUserId",
                table: "DatabaseBook",
                column: "DatabaseUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseBook_AspNetUsers_DatabaseUserId",
                table: "DatabaseBook",
                column: "DatabaseUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseBook_DatabaseAuthor_DatabaseAuthorId",
                table: "DatabaseBook",
                column: "DatabaseAuthorId",
                principalTable: "DatabaseAuthor",
                principalColumn: "Id");
        }
    }
}
