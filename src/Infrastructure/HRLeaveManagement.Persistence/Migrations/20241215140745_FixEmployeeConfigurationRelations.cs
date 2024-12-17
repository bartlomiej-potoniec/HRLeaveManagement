using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployeeConfigurationRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_LeaderId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SectionId",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LeaderId",
                table: "Employees",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SectionId",
                table: "Employees",
                column: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_LeaderId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SectionId",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LeaderId",
                table: "Employees",
                column: "LeaderId",
                unique: true,
                filter: "[LeaderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SectionId",
                table: "Employees",
                column: "SectionId",
                unique: true);
        }
    }
}
