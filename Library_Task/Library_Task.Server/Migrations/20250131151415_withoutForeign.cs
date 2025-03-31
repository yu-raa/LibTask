using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Task.Server.Migrations
{
    /// <inheritdoc />
    public partial class withoutForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseBook_AspNetUsers_UserId",
                table: "DatabaseBook");

            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseBook_DatabaseAuthor_AuthorId",
                table: "DatabaseBook");

            migrationBuilder.DropIndex(
                name: "IX_DatabaseBook_AuthorId",
                table: "DatabaseBook");

            migrationBuilder.DropIndex(
                name: "IX_DatabaseBook_UserId",
                table: "DatabaseBook");

            migrationBuilder.DropColumn(
                name: "AspNetUsers",
                table: "DatabaseBook");

            migrationBuilder.DropColumn(
                name: "DatabaseAuthor",
                table: "DatabaseBook");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DatabaseBook",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "DatabaseBook",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AspNetUsers",
                table: "DatabaseBook",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabaseAuthor",
                table: "DatabaseBook",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseBook_AuthorId",
                table: "DatabaseBook",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseBook_UserId",
                table: "DatabaseBook",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseBook_AspNetUsers_UserId",
                table: "DatabaseBook",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseBook_DatabaseAuthor_AuthorId",
                table: "DatabaseBook",
                column: "AuthorId",
                principalTable: "DatabaseAuthor",
                principalColumn: "Id");
        }
    }
}
