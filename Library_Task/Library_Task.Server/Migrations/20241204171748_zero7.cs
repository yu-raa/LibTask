using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Task.Server.Migrations
{
    /// <inheritdoc />
    public partial class zero7 : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseBook_AspNetUsers_UserId",
                table: "DatabaseBook");

            migrationBuilder.DropForeignKey(
                name: "FK_DatabaseBook_DatabaseAuthor_AuthorId",
                table: "DatabaseBook");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "DatabaseBook",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
    }
}
