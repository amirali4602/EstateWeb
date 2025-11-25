using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedRentDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PriceMeter",
                table: "Pages",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "isRent",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceMeter",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "isRent",
                table: "Pages");
        }
    }
}
