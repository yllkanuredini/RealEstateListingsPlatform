using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateListingPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyImage",
                table: "Properties",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyImage",
                table: "Properties");
        }
    }
}
