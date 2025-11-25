using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AdsMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdsMessage",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdsMessage",
                table: "Pages");
        }
    }
}
