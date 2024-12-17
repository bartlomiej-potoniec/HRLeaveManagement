using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployeeAndSectionIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LeaderId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_LeaderId",
                table: "Sections",
                column: "LeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Employees_LeaderId",
                table: "Sections",
                column: "LeaderId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Employees_LeaderId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_LeaderId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "Sections");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
