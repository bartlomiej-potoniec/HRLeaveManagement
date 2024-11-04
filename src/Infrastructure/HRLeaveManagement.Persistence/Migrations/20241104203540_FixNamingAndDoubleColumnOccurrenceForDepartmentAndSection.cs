using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixNamingAndDoubleColumnOccurrenceForDepartmentAndSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_Employees_LeaderId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Section_SectionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_Department_DepartmentId",
                table: "Section");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_Department_DepartmentId1",
                table: "Section");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Section",
                table: "Section");

            migrationBuilder.DropIndex(
                name: "IX_Section_DepartmentId1",
                table: "Section");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "Section");

            migrationBuilder.RenameTable(
                name: "Section",
                newName: "Sections");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.RenameIndex(
                name: "IX_Section_DepartmentId",
                table: "Sections",
                newName: "IX_Sections_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Department_LeaderId",
                table: "Departments",
                newName: "IX_Departments_LeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sections",
                table: "Sections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_LeaderId",
                table: "Departments",
                column: "LeaderId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Sections_SectionId",
                table: "Employees",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Departments_DepartmentId",
                table: "Sections",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_LeaderId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Sections_SectionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Departments_DepartmentId",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sections",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.RenameTable(
                name: "Sections",
                newName: "Section");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_DepartmentId",
                table: "Section",
                newName: "IX_Section_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_LeaderId",
                table: "Department",
                newName: "IX_Department_LeaderId");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "Section",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Section",
                table: "Section",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Section_DepartmentId1",
                table: "Section",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Employees_LeaderId",
                table: "Department",
                column: "LeaderId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Section_SectionId",
                table: "Employees",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Department_DepartmentId",
                table: "Section",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Department_DepartmentId1",
                table: "Section",
                column: "DepartmentId1",
                principalTable: "Department",
                principalColumn: "Id");
        }
    }
}
