using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRLeaveManagement.Identity.Migrations
{
    /// <inheritdoc />
    public partial class ImproveUserModelAndAddMoreRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ae1e9edb-d020-4fd4-8c39-eb6557aee3c2", "9a9a289c-56e5-4dd6-917e-d892b381f815" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9a9a289c-56e5-4dd6-917e-d892b381f815");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PeselNumber",
                table: "AspNetUsers",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19f6a619-c49c-4a46-87f7-cd58478af056", null, "Manager", "MANAGER" },
                    { "5746cb95-1b05-4231-8e87-3648e4fb9208", null, "HR", "HR" },
                    { "83476db7-e703-4b28-9c17-7ee12a2ddab1", null, "CEO", "CEO" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ed62efd7-a3da-49de-b702-c422ec7b0165",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "EmployeeId", "NormalizedUserName", "PasswordHash", "PeselNumber", "SecurityStamp", "UserName" },
                values: new object[] { "eebba96d-e331-44bf-964b-c1265deb947c", new DateOnly(1, 1, 1), null, "ADMIN", "AQAAAAIAAYagAAAAEPsG8G/wzOyDll3HgDP5IcgcwC0PUrt6XPCRVeDZJv/8NEKlxv0NPU6UU9YdLYUdZw==", null, "11a6fb51-c087-46e6-b465-8dd3813e43b1", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19f6a619-c49c-4a46-87f7-cd58478af056");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5746cb95-1b05-4231-8e87-3648e4fb9208");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83476db7-e703-4b28-9c17-7ee12a2ddab1");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PeselNumber",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ed62efd7-a3da-49de-b702-c422ec7b0165",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "b2439750-7a12-4494-9453-d5aea7531abe", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEMLC9IKWe90VfEz8RxqcZBehSDTWRQhyk+4lekpaBUKEhL4eUCdFWEVstfHv/pIY3Q==", "0cdc0267-d1a5-4e38-bdb3-7c06ae0115f9", "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "9a9a289c-56e5-4dd6-917e-d892b381f815", 0, "419d40ff-d079-4443-b0d2-46570c9e5a89", "user@localhost.com", true, "System", "User", false, null, "USER@LOCALHOST.COM", "USER@LOCALHOST.COM", "AQAAAAIAAYagAAAAEBhJQu+GQlx3ZDz/7xe6yqRa/zGYti8PkCvtH/fM8Eq7unVRLc55QkU6KUlL9fMoUQ==", null, false, "69d3f10c-31df-4959-b7d4-8a5e94a3de61", false, "user@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ae1e9edb-d020-4fd4-8c39-eb6557aee3c2", "9a9a289c-56e5-4dd6-917e-d892b381f815" });
        }
    }
}
