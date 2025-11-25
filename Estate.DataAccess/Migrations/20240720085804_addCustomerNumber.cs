using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCustomerNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerNumber",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ShowCustomerNumber",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerNumber",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ShowCustomerNumber",
                table: "Pages");
        }
    }
}
