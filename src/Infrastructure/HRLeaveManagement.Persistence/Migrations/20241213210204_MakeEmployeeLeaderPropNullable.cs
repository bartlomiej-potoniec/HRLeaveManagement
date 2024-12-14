using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeEmployeeLeaderPropNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_LeaderId",
                table: "Employees");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeaderId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LeaderId",
                table: "Employees",
                column: "LeaderId",
                unique: true,
                filter: "[LeaderId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_LeaderId",
                table: "Employees");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeaderId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LeaderId",
                table: "Employees",
                column: "LeaderId",
                unique: true);
        }
    }
}
