using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTptForeignKeyIssueForWorkRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DelegationRequest_Employees_SubstitutorId",
                table: "DelegationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_DelegationRequest_WorkRequest_Id",
                table: "DelegationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtraRemoteWorkRequest_WorkRequest_Id",
                table: "ExtraRemoteWorkRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeRequest_WorkRequest_Id",
                table: "OvertimeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkRequest_Employees_ApproverId",
                table: "WorkRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkRequest_Employees_RequestingEmployeeId",
                table: "WorkRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkRequest",
                table: "WorkRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OvertimeRequest",
                table: "OvertimeRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtraRemoteWorkRequest",
                table: "ExtraRemoteWorkRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DelegationRequest",
                table: "DelegationRequest");

            migrationBuilder.RenameTable(
                name: "WorkRequest",
                newName: "WorkRequests");

            migrationBuilder.RenameTable(
                name: "OvertimeRequest",
                newName: "OvertimeRequests");

            migrationBuilder.RenameTable(
                name: "ExtraRemoteWorkRequest",
                newName: "ExtraRemoteWorkRequests");

            migrationBuilder.RenameTable(
                name: "DelegationRequest",
                newName: "DelegationRequests");

            migrationBuilder.RenameIndex(
                name: "IX_WorkRequest_RequestingEmployeeId",
                table: "WorkRequests",
                newName: "IX_WorkRequests_RequestingEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkRequest_ApproverId",
                table: "WorkRequests",
                newName: "IX_WorkRequests_ApproverId");

            migrationBuilder.RenameIndex(
                name: "IX_DelegationRequest_SubstitutorId",
                table: "DelegationRequests",
                newName: "IX_DelegationRequests_SubstitutorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkRequests",
                table: "WorkRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OvertimeRequests",
                table: "OvertimeRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtraRemoteWorkRequests",
                table: "ExtraRemoteWorkRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DelegationRequests",
                table: "DelegationRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegationRequests_Employees_SubstitutorId",
                table: "DelegationRequests",
                column: "SubstitutorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegationRequests_WorkRequests_Id",
                table: "DelegationRequests",
                column: "Id",
                principalTable: "WorkRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraRemoteWorkRequests_WorkRequests_Id",
                table: "ExtraRemoteWorkRequests",
                column: "Id",
                principalTable: "WorkRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeRequests_WorkRequests_Id",
                table: "OvertimeRequests",
                column: "Id",
                principalTable: "WorkRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkRequests_Employees_ApproverId",
                table: "WorkRequests",
                column: "ApproverId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkRequests_Employees_RequestingEmployeeId",
                table: "WorkRequests",
                column: "RequestingEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DelegationRequests_Employees_SubstitutorId",
                table: "DelegationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DelegationRequests_WorkRequests_Id",
                table: "DelegationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtraRemoteWorkRequests_WorkRequests_Id",
                table: "ExtraRemoteWorkRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeRequests_WorkRequests_Id",
                table: "OvertimeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkRequests_Employees_ApproverId",
                table: "WorkRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkRequests_Employees_RequestingEmployeeId",
                table: "WorkRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkRequests",
                table: "WorkRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OvertimeRequests",
                table: "OvertimeRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtraRemoteWorkRequests",
                table: "ExtraRemoteWorkRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DelegationRequests",
                table: "DelegationRequests");

            migrationBuilder.RenameTable(
                name: "WorkRequests",
                newName: "WorkRequest");

            migrationBuilder.RenameTable(
                name: "OvertimeRequests",
                newName: "OvertimeRequest");

            migrationBuilder.RenameTable(
                name: "ExtraRemoteWorkRequests",
                newName: "ExtraRemoteWorkRequest");

            migrationBuilder.RenameTable(
                name: "DelegationRequests",
                newName: "DelegationRequest");

            migrationBuilder.RenameIndex(
                name: "IX_WorkRequests_RequestingEmployeeId",
                table: "WorkRequest",
                newName: "IX_WorkRequest_RequestingEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkRequests_ApproverId",
                table: "WorkRequest",
                newName: "IX_WorkRequest_ApproverId");

            migrationBuilder.RenameIndex(
                name: "IX_DelegationRequests_SubstitutorId",
                table: "DelegationRequest",
                newName: "IX_DelegationRequest_SubstitutorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkRequest",
                table: "WorkRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OvertimeRequest",
                table: "OvertimeRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtraRemoteWorkRequest",
                table: "ExtraRemoteWorkRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DelegationRequest",
                table: "DelegationRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegationRequest_Employees_SubstitutorId",
                table: "DelegationRequest",
                column: "SubstitutorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegationRequest_WorkRequest_Id",
                table: "DelegationRequest",
                column: "Id",
                principalTable: "WorkRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraRemoteWorkRequest_WorkRequest_Id",
                table: "ExtraRemoteWorkRequest",
                column: "Id",
                principalTable: "WorkRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeRequest_WorkRequest_Id",
                table: "OvertimeRequest",
                column: "Id",
                principalTable: "WorkRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkRequest_Employees_ApproverId",
                table: "WorkRequest",
                column: "ApproverId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkRequest_Employees_RequestingEmployeeId",
                table: "WorkRequest",
                column: "RequestingEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
