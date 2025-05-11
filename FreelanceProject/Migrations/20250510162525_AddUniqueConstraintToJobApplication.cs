using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelanceProject.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToJobApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobApplications_ApplicantId",
                table: "JobApplications");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ApplicantId_JobId",
                table: "JobApplications",
                columns: new[] { "ApplicantId", "JobId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobApplications_ApplicantId_JobId",
                table: "JobApplications");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ApplicantId",
                table: "JobApplications",
                column: "ApplicantId");
        }
    }
}
