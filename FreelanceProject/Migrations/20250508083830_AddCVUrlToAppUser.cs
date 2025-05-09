using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelanceProject.Migrations
{
    /// <inheritdoc />
    public partial class AddCVUrlToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVUrl",
                table: "AspNetUsers");
        }
    }
}
