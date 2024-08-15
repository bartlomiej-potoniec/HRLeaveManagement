using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "LeaveRequests",
                newName: "StartedAt");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "LeaveRequests",
                newName: "EndedAt");

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 6, 9, 19, 0, 44, 456, DateTimeKind.Local).AddTicks(8864), new DateTime(2024, 6, 9, 19, 0, 44, 456, DateTimeKind.Local).AddTicks(8917) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "LeaveRequests",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndedAt",
                table: "LeaveRequests",
                newName: "EndDate");

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 5, 12, 23, 51, 31, 590, DateTimeKind.Local).AddTicks(6064), new DateTime(2024, 5, 12, 23, 51, 31, 590, DateTimeKind.Local).AddTicks(6120) });
        }
    }
}
