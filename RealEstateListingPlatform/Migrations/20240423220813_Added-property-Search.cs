using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RealEstateListingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddedpropertySearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inquiries_User_UserId",
                table: "Inquiries");

            migrationBuilder.DropForeignKey(
                name: "FK_Viewings_User_UserId",
                table: "Viewings");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Viewings_UserId",
                table: "Viewings");

            migrationBuilder.DropIndex(
                name: "IX_Inquiries_UserId",
                table: "Inquiries");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Viewings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Inquiries",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Viewings_UserId1",
                table: "Viewings",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_UserId1",
                table: "Inquiries",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiries_AspNetUsers_UserId1",
                table: "Inquiries",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Viewings_AspNetUsers_UserId1",
                table: "Viewings",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inquiries_AspNetUsers_UserId1",
                table: "Inquiries");

            migrationBuilder.DropForeignKey(
                name: "FK_Viewings_AspNetUsers_UserId1",
                table: "Viewings");

            migrationBuilder.DropIndex(
                name: "IX_Viewings_UserId1",
                table: "Viewings");

            migrationBuilder.DropIndex(
                name: "IX_Inquiries_UserId1",
                table: "Inquiries");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Viewings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Inquiries");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConfirmPassword = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Viewings_UserId",
                table: "Viewings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_UserId",
                table: "Inquiries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiries_User_UserId",
                table: "Inquiries",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Viewings_User_UserId",
                table: "Viewings",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
