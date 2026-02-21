using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YatirimKoc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_AdminProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "IsAppointmentEnabled",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "IsVisibleOnWebsite",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AdminProfiles");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "AdminProfiles",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "About",
                table: "AdminProfiles",
                newName: "Biography");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AdminProfiles",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ExperienceYear",
                table: "AdminProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "AdminProfiles",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AdminProfiles",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminProfiles_Users_UserId",
                table: "AdminProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminProfiles_Users_UserId",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "ExperienceYear",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "AdminProfiles");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AdminProfiles");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AdminProfiles",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Biography",
                table: "AdminProfiles",
                newName: "About");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AdminProfiles",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "AdminProfiles",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "AdminProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAppointmentEnabled",
                table: "AdminProfiles",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleOnWebsite",
                table: "AdminProfiles",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AdminProfiles",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "AdminProfiles",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }
    }
}
