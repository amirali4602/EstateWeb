using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Estate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeAgents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Agents_AgentId",
                table: "Pages");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropIndex(
                name: "IX_Pages_AgentId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Pages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "Description", "Name", "Number", "ProfilePic" },
                values: new object[,]
                {
                    { 1, "0", "احسان بخش", "", "0" },
                    { 2, "0", "ملکی", "", "0" },
                    { 3, "0", "ابراهیمی", "", "0" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pages_AgentId",
                table: "Pages",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Agents_AgentId",
                table: "Pages",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
