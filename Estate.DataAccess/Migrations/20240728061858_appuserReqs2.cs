using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class appuserReqs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "instagram",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isOnlyRent",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "telegram",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "whatsApp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "instagram",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "isOnlyRent",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "telegram",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "whatsApp",
                table: "AspNetUsers");
        }
    }
}
