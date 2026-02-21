using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YatirimKoc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ListingModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ListingCategories_ListingCategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_ListingTypes_ListingTypeId",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "ListingCategories");

            migrationBuilder.DropTable(
                name: "ListingTypes");

            migrationBuilder.DropIndex(
                name: "IX_Listings_City_District",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_CreatedByUserId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_IsPublished",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_Price",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_Slug",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "BathroomCount",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "BuildingAge",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "RoomCount",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "SquareMeter",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "TotalFloor",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                table: "Listings",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "ListingTypeId",
                table: "Listings",
                newName: "TransactionTypeId");

            migrationBuilder.RenameColumn(
                name: "ListingCategoryId",
                table: "Listings",
                newName: "PropertyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_ListingTypeId",
                table: "Listings",
                newName: "IX_Listings_TransactionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_ListingCategoryId",
                table: "Listings",
                newName: "IX_Listings_PropertyTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Listings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Listings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Listings",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

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

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Listings",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

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

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Listings",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Listings",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InputType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Options = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PropertyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyTypes_PropertyTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ListingFeatureValues",
                columns: table => new
                {
                    ListingId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FeatureId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingFeatureValues", x => new { x.ListingId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_ListingFeatureValues_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListingFeatureValues_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PropertyFeatures",
                columns: table => new
                {
                    PropertyTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FeatureId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyFeatures", x => new { x.PropertyTypeId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_PropertyFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyFeatures_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ListingFeatureValues_FeatureId",
                table: "ListingFeatureValues",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyFeatures_FeatureId",
                table: "PropertyFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTypes_ParentId",
                table: "PropertyTypes",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_PropertyTypes_PropertyTypeId",
                table: "Listings",
                column: "PropertyTypeId",
                principalTable: "PropertyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_TransactionTypes_TransactionTypeId",
                table: "Listings",
                column: "TransactionTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_PropertyTypes_PropertyTypeId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_TransactionTypes_TransactionTypeId",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "ListingFeatureValues");

            migrationBuilder.DropTable(
                name: "PropertyFeatures");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "PropertyTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Listings",
                newName: "PublishedAt");

            migrationBuilder.RenameColumn(
                name: "TransactionTypeId",
                table: "Listings",
                newName: "ListingTypeId");

            migrationBuilder.RenameColumn(
                name: "PropertyTypeId",
                table: "Listings",
                newName: "ListingCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_TransactionTypeId",
                table: "Listings",
                newName: "IX_Listings_ListingTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_PropertyTypeId",
                table: "Listings",
                newName: "IX_Listings_ListingCategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Listings",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Listings",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Listings",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

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

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Listings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

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

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomCount",
                table: "Listings",
                type: "int",
                nullable: true);

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
                name: "ListingCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ListingTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_City_District",
                table: "Listings",
                columns: new[] { "City", "District" });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedByUserId",
                table: "Listings",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_IsPublished",
                table: "Listings",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Price",
                table: "Listings",
                column: "Price");

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
                name: "IX_ListingTypes_Name",
                table: "ListingTypes",
                column: "Name",
                unique: true);

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
    }
}
