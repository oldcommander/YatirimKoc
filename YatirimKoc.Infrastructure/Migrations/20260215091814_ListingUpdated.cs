using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YatirimKoc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ListingUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AdminProfiles_AdminProfileId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ListingCategories_ListingCategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ListingTypes_ListingTypeId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ExternalUrl",
                table: "Listings");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SiteSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Neighborhood",
                table: "Listings",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Listings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Listings",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Listings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "BathroomCount",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuildingAge",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Listings",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Listings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Listings",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Listings",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Listings",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomCount",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Listings",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SquareMeter",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalFloor",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ListingImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ListingId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ImageUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsCover = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingImages_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ListingTypes_Name",
                table: "ListingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_City_District",
                table: "Listings",
                columns: new[] { "City", "District" });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedByUserId",
                table: "Listings",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Slug",
                table: "Listings",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListingCategories_Name",
                table: "ListingCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListingImages_ListingId",
                table: "ListingImages",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AdminProfiles_AdminProfileId",
                table: "Listings",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ListingCategories_ListingCategoryId",
                table: "Listings",
                column: "ListingCategoryId",
                principalTable: "ListingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ListingTypes_ListingTypeId",
                table: "Listings",
                column: "ListingTypeId",
                principalTable: "ListingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AdminProfiles_AdminProfileId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ListingCategories_ListingCategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ListingTypes_ListingTypeId",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "ListingImages");

            migrationBuilder.DropIndex(
                name: "IX_ListingTypes_Name",
                table: "ListingTypes");

            migrationBuilder.DropIndex(
                name: "IX_Listings_City_District",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_CreatedByUserId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_Slug",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_ListingCategories_Name",
                table: "ListingCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "BathroomCount",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "BuildingAge",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "RoomCount",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "SquareMeter",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "TotalFloor",
                table: "Listings");

            migrationBuilder.AlterColumn<string>(
                name: "Neighborhood",
                table: "Listings",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Listings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Listings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Listings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrl",
                table: "Listings",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AdminProfiles_AdminProfileId",
                table: "Listings",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ListingCategories_ListingCategoryId",
                table: "Listings",
                column: "ListingCategoryId",
                principalTable: "ListingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_ListingTypes_ListingTypeId",
                table: "Listings",
                column: "ListingTypeId",
                principalTable: "ListingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
