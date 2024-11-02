using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRLeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RestructureLeaveTablesAndAddNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveAllocations_LeaveTypes_LeaveTypeId",
                table: "LeaveAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "DefaultDays",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "RequestComment",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "RequestedDate",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "NumberOfDays",
                table: "LeaveAllocations");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "LeaveRequests",
                newName: "DecidedAt");

            migrationBuilder.RenameColumn(
                name: "Period",
                table: "LeaveAllocations",
                newName: "Year");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LeaveTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "LeaveTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LeaveTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidFraction",
                table: "LeaveTypes",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartedAt",
                table: "LeaveRequests",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestingEmployeeId",
                table: "LeaveRequests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndedAt",
                table: "LeaveRequests",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproverComment",
                table: "LeaveRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApproverId",
                table: "LeaveRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "LeaveRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentPath",
                table: "LeaveRequests",
                type: "nvarchar(260)",
                maxLength: 260,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonDescription",
                table: "LeaveRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LeaveRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SubstitutorId",
                table: "LeaveRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TotalDays",
                table: "LeaveRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "LeaveAllocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "LeaveAllocations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveAllocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvailableDays",
                table: "LeaveAllocations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemainingDays",
                table: "LeaveAllocations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsedDays",
                table: "LeaveAllocations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DelegationRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SubstitutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationCountry = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    MeansOfTransport = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CashAdvance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurposeDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegationRequest", x => x.Id);
                    table.CheckConstraint("CK_DelegationRequest_CashAdvance_GreaterThanOrEqualToZero", "[CashAdvance] >= 0.0");
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Section_Department_DepartmentId1",
                        column: x => x.DepartmentId1,
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Responsibilities = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    LeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractType = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiredAt = table.Column<DateOnly>(type: "date", nullable: true),
                    TotalDuration = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeContracts", x => x.Id);
                    table.CheckConstraint("CK_EmployeeContract_TotalDuration_GreaterThanZero", "[TotalDuration] IS NULL OR [TotalDuration] > 0");
                    table.ForeignKey(
                        name: "FK_EmployeeContracts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeContracts_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEducations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EducationType = table.Column<int>(type: "int", nullable: false),
                    EducationDetails = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EnrolledAt = table.Column<DateOnly>(type: "date", nullable: false),
                    GraduatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeEducations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeEducations_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractType = table.Column<int>(type: "int", nullable: false),
                    EmployedFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    EmployedTo = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalEmployment = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeExperiences", x => x.Id);
                    table.CheckConstraint("CK_EmployeeExperience_TotalEmployment_GreaterThanZero", "[TotalEmployment] > 0");
                    table.ForeignKey(
                        name: "FK_EmployeeExperiences_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeExperiences_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RemoteWorkLimits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    AvailableDays = table.Column<int>(type: "int", nullable: false),
                    UsedDays = table.Column<int>(type: "int", nullable: false),
                    RemainingDays = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemoteWorkLimits", x => x.Id);
                    table.CheckConstraint("CK_RemoteWorkLimit_AvailableDays_GreaterThanOrEqualToZero", "[AvailableDays] >= 0");
                    table.CheckConstraint("CK_RemoteWorkLimit_RemainingDays_FewerThanOrEqualToAvailableDays", "[RemainingDays] <= [AvailableDays]");
                    table.CheckConstraint("CK_RemoteWorkLimit_UsedDays_FewerThanOrEqualToAvailableDays", "[UsedDays] <= [AvailableDays]");
                    table.CheckConstraint("CK_RemoteWorkLimit_Year_GreaterThanOrEqualToCurrent", "[Year] >= YEAR(GETDATE())");
                    table.ForeignKey(
                        name: "FK_RemoteWorkLimits_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TimeRegisters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisterDate = table.Column<DateOnly>(type: "date", nullable: false),
                    WorkStartedAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    WorkEndedAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    TotalWorkTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    BreakStartedAt = table.Column<TimeOnly>(type: "time", nullable: true),
                    BreakEndedAt = table.Column<TimeOnly>(type: "time", nullable: true),
                    TotalBreakTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeRegisters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeRegisters_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestingEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    EndedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalDays = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApproverComment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecidedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRequest", x => x.Id);
                    table.CheckConstraint("CK_WorkRequest_EndedAt_GreaterThanOrEqualToStartedAt", "[EndedAt] >= [StartedAt]");
                    table.CheckConstraint("CK_WorkRequest_StartedAt_GreaterThanOrEqualToToday", "[StartedAt] >= CAST(GETDATE() AS date)");
                    table.CheckConstraint("CK_WorkRequest_TotalDays_GreaterThanOrEqualToZero", "[TotalDays] >= 0");
                    table.ForeignKey(
                        name: "FK_WorkRequest_Employees_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkRequest_Employees_RequestingEmployeeId",
                        column: x => x.RequestingEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExtraRemoteWorkRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ReasonDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraRemoteWorkRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraRemoteWorkRequest_WorkRequest_Id",
                        column: x => x.Id,
                        principalTable: "WorkRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OvertimeRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PurposeDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OvertimeRequest_WorkRequest_Id",
                        column: x => x.Id,
                        principalTable: "WorkRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "LeaveType_PaidFraction_BetweenZeroAndOne",
                table: "LeaveTypes",
                sql: "[PaidFraction] BETWEEN 0.0 AND 1.0");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_ApproverId",
                table: "LeaveRequests",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_RequestingEmployeeId",
                table: "LeaveRequests",
                column: "RequestingEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_SubstitutorId",
                table: "LeaveRequests",
                column: "SubstitutorId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveRequest_EndedAt_GreaterThanOrEqualToStartedAt",
                table: "LeaveRequests",
                sql: "[EndedAt] >= StartedAt");

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveRequest_StartedAt_GreaterThanOrEqualToToday",
                table: "LeaveRequests",
                sql: "[StartedAt] >= CAST(GETDATE() AS date)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveRequest_TotalDays_GreaterThanZero",
                table: "LeaveRequests",
                sql: "[TotalDays] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAllocations_EmployeeId",
                table: "LeaveAllocations",
                column: "EmployeeId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveAllocation_AvailableDays_GreaterThanOrEqualToZero",
                table: "LeaveAllocations",
                sql: "[AvailableDays] IS NULL OR [AvailableDays] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveAllocation_RemainingDays_FewerThanOrEqualToAvailableDays",
                table: "LeaveAllocations",
                sql: "[RemainingDays] IS NULL OR [RemainingDays] <= [AvailableDays]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveAllocation_UsedDays_FewerThanOrEqualToAvailableDays",
                table: "LeaveAllocations",
                sql: "[UsedDays] IS NULL OR [UsedDays] <= [AvailableDays]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveAllocation_Year_GreaterThanOrEqualToCurrent",
                table: "LeaveAllocations",
                sql: "[Year] >= YEAR(GETDATE())");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationRequest_SubstitutorId",
                table: "DelegationRequest",
                column: "SubstitutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_LeaderId",
                table: "Department",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContracts_EmployeeId",
                table: "EmployeeContracts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContracts_EmployeeId1",
                table: "EmployeeContracts",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EmployeeId",
                table: "EmployeeEducations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EmployeeId1",
                table: "EmployeeEducations",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExperiences_EmployeeId",
                table: "EmployeeExperiences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExperiences_EmployeeId1",
                table: "EmployeeExperiences",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LeaderId",
                table: "Employees",
                column: "LeaderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SectionId",
                table: "Employees",
                column: "SectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RemoteWorkLimits_EmployeeId",
                table: "RemoteWorkLimits",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_DepartmentId",
                table: "Section",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_DepartmentId1",
                table: "Section",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRegisters_EmployeeId",
                table: "TimeRegisters",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRequest_ApproverId",
                table: "WorkRequest",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRequest_RequestingEmployeeId",
                table: "WorkRequest",
                column: "RequestingEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveAllocations_Employees_EmployeeId",
                table: "LeaveAllocations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveAllocations_LeaveTypes_LeaveTypeId",
                table: "LeaveAllocations",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_Employees_ApproverId",
                table: "LeaveRequests",
                column: "ApproverId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_Employees_RequestingEmployeeId",
                table: "LeaveRequests",
                column: "RequestingEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_Employees_SubstitutorId",
                table: "LeaveRequests",
                column: "SubstitutorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id");

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
                name: "FK_Department_Employees_LeaderId",
                table: "Department",
                column: "LeaderId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveAllocations_Employees_EmployeeId",
                table: "LeaveAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveAllocations_LeaveTypes_LeaveTypeId",
                table: "LeaveAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_Employees_ApproverId",
                table: "LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_Employees_RequestingEmployeeId",
                table: "LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_Employees_SubstitutorId",
                table: "LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Employees_LeaderId",
                table: "Department");

            migrationBuilder.DropTable(
                name: "DelegationRequest");

            migrationBuilder.DropTable(
                name: "EmployeeContracts");

            migrationBuilder.DropTable(
                name: "EmployeeEducations");

            migrationBuilder.DropTable(
                name: "EmployeeExperiences");

            migrationBuilder.DropTable(
                name: "ExtraRemoteWorkRequest");

            migrationBuilder.DropTable(
                name: "OvertimeRequest");

            migrationBuilder.DropTable(
                name: "RemoteWorkLimits");

            migrationBuilder.DropTable(
                name: "TimeRegisters");

            migrationBuilder.DropTable(
                name: "WorkRequest");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropCheckConstraint(
                name: "LeaveType_PaidFraction_BetweenZeroAndOne",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_ApproverId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_RequestingEmployeeId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_SubstitutorId",
                table: "LeaveRequests");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveRequest_EndedAt_GreaterThanOrEqualToStartedAt",
                table: "LeaveRequests");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveRequest_StartedAt_GreaterThanOrEqualToToday",
                table: "LeaveRequests");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveRequest_TotalDays_GreaterThanZero",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveAllocations_EmployeeId",
                table: "LeaveAllocations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveAllocation_AvailableDays_GreaterThanOrEqualToZero",
                table: "LeaveAllocations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveAllocation_RemainingDays_FewerThanOrEqualToAvailableDays",
                table: "LeaveAllocations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveAllocation_UsedDays_FewerThanOrEqualToAvailableDays",
                table: "LeaveAllocations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveAllocation_Year_GreaterThanOrEqualToCurrent",
                table: "LeaveAllocations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "PaidFraction",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "ApproverComment",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "DocumentPath",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "ReasonDescription",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "SubstitutorId",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "AvailableDays",
                table: "LeaveAllocations");

            migrationBuilder.DropColumn(
                name: "RemainingDays",
                table: "LeaveAllocations");

            migrationBuilder.DropColumn(
                name: "UsedDays",
                table: "LeaveAllocations");

            migrationBuilder.RenameColumn(
                name: "DecidedAt",
                table: "LeaveRequests",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "LeaveAllocations",
                newName: "Period");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LeaveTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "LeaveTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "DefaultDays",
                table: "LeaveTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedAt",
                table: "LeaveRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "RequestingEmployeeId",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndedAt",
                table: "LeaveRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "LeaveRequests",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "LeaveRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RequestComment",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDate",
                table: "LeaveRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "LeaveAllocations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "LeaveAllocations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LeaveAllocations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDays",
                table: "LeaveAllocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "CreatedAt", "DefaultDays", "ModifiedAt", "Name" },
                values: new object[] { 1, new DateTime(2024, 6, 9, 19, 0, 44, 456, DateTimeKind.Local).AddTicks(8864), 10, new DateTime(2024, 6, 9, 19, 0, 44, 456, DateTimeKind.Local).AddTicks(8917), "Vacation" });

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveAllocations_LeaveTypes_LeaveTypeId",
                table: "LeaveAllocations",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
