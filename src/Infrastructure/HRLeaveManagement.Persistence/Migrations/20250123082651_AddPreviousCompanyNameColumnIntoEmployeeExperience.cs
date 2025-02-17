using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPreviousCompanyNameColumnIntoEmployeeExperience : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviousCompanyName",
                table: "EmployeeExperiences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "DefaultValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousCompanyName",
                table: "EmployeeExperiences");
        }
    }
}
