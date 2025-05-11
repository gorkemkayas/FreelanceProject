using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelanceProject.Migrations
{
    /// <inheritdoc />
    public partial class FixMessageEntityRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicantId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Jobs_JobId",
                table: "JobApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications");

            migrationBuilder.RenameTable(
                name: "JobApplications",
                newName: "JobApplicationEntity");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_JobId",
                table: "JobApplicationEntity",
                newName: "IX_JobApplicationEntity_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_ApplicantId_JobId",
                table: "JobApplicationEntity",
                newName: "IX_JobApplicationEntity_ApplicantId_JobId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplicationEntity",
                table: "JobApplicationEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicationEntity_AspNetUsers_ApplicantId",
                table: "JobApplicationEntity",
                column: "ApplicantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicationEntity_Jobs_JobId",
                table: "JobApplicationEntity",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplicationEntity_AspNetUsers_ApplicantId",
                table: "JobApplicationEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplicationEntity_Jobs_JobId",
                table: "JobApplicationEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplicationEntity",
                table: "JobApplicationEntity");

            migrationBuilder.RenameTable(
                name: "JobApplicationEntity",
                newName: "JobApplications");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplicationEntity_JobId",
                table: "JobApplications",
                newName: "IX_JobApplications_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplicationEntity_ApplicantId_JobId",
                table: "JobApplications",
                newName: "IX_JobApplications_ApplicantId_JobId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicantId",
                table: "JobApplications",
                column: "ApplicantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Jobs_JobId",
                table: "JobApplications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
