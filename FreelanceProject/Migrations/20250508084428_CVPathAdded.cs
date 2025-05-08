using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelanceProject.Migrations
{
    /// <inheritdoc />
    public partial class CVPathAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVPath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVPath",
                table: "AspNetUsers");
        }
    }
}
