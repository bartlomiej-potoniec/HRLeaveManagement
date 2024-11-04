using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixDoubleColumnOccurrenceInEmployeeInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeContracts_Employees_EmployeeId1",
                table: "EmployeeContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeEducations_Employees_EmployeeId1",
                table: "EmployeeEducations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeExperiences_Employees_EmployeeId1",
                table: "EmployeeExperiences");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeExperiences_EmployeeId1",
                table: "EmployeeExperiences");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeEducations_EmployeeId1",
                table: "EmployeeEducations");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeContracts_EmployeeId1",
                table: "EmployeeContracts");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "EmployeeExperiences");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "EmployeeEducations");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "EmployeeContracts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                table: "EmployeeExperiences",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                table: "EmployeeEducations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                table: "EmployeeContracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExperiences_EmployeeId1",
                table: "EmployeeExperiences",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EmployeeId1",
                table: "EmployeeEducations",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContracts_EmployeeId1",
                table: "EmployeeContracts",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeContracts_Employees_EmployeeId1",
                table: "EmployeeContracts",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeEducations_Employees_EmployeeId1",
                table: "EmployeeEducations",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeExperiences_Employees_EmployeeId1",
                table: "EmployeeExperiences",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
