using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Task.Server.Migrations
{
    /// <inheritdoc />
    public partial class zero6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseBook_AspNetUsers_AspNetUsers",
                table: "DatabaseBook");

            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseBook_DatabaseAuthor_DatabaseAuthor",
                table: "DatabaseBook");

            migrationBuilder.DropIndex(
                name: "IX_DatabaseBook_AspNetUsers",
                table: "DatabaseBook");

            migrationBuilder.DropIndex(
                name: "IX_DatabaseBook_DatabaseAuthor",
                table: "DatabaseBook");

            migrationBuilder.AlterColumn<string>(
                name: "DatabaseAuthor",
                table: "DatabaseBook",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AspNetUsers",
                table: "DatabaseBook",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseBook_DatabaseAuthor_AuthorId",
                table: "DatabaseBook",
                column: "AuthorId",
                principalTable: "DatabaseAuthor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "AuthorId",
                table: "DatabaseBook");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DatabaseBook");

            migrationBuilder.AlterColumn<string>(
                name: "DatabaseAuthor",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AspNetUsers",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseBook_AspNetUsers",
                table: "DatabaseBook",
                column: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseBook_DatabaseAuthor",
                table: "DatabaseBook",
                column: "DatabaseAuthor");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseBook_AspNetUsers_AspNetUsers",
                table: "DatabaseBook",
                column: "AspNetUsers",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DatabaseBook_DatabaseAuthor_DatabaseAuthor",
                table: "DatabaseBook",
                column: "DatabaseAuthor",
                principalTable: "DatabaseAuthor",
                principalColumn: "Id");
        }
    }
}
