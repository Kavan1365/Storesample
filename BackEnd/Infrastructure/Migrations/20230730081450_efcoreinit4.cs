using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class efcoreinit4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutStore",
                table: "Setting",
                type: "nvarchar(max)",
                maxLength: 2000000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Setting",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Setting",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactUS",
                table: "Setting",
                type: "nvarchar(max)",
                maxLength: 2000000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFreeSender",
                table: "Setting",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPayInLocation",
                table: "Setting",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaybyUser",
                table: "Setting",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "PriceAsStore",
                table: "Setting",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PriceFree",
                table: "Setting",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PriceOtherProvince",
                table: "Setting",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Transportation",
                table: "Setting",
                type: "bigint",
                maxLength: 2000000,
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HexCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUSList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    StatusContract = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUSList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefinedProductId = table.Column<int>(type: "int", nullable: false),
                    ProductPriceType = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrice_Color_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Color",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductPrice_DefinedProduct_DefinedProductId",
                        column: x => x.DefinedProductId,
                        principalSchema: "PPP",
                        principalTable: "DefinedProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Setting_CityId",
                table: "Setting",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Color_Guid",
                table: "Color",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactUSList_Guid",
                table: "ContactUSList",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrice_ColorId",
                table: "ProductPrice",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrice_DefinedProductId",
                table: "ProductPrice",
                column: "DefinedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrice_Guid",
                table: "ProductPrice",
                column: "Guid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_City_CityId",
                table: "Setting",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_City_CityId",
                table: "Setting");

            migrationBuilder.DropTable(
                name: "ContactUSList");

            migrationBuilder.DropTable(
                name: "ProductPrice");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropIndex(
                name: "IX_Setting_CityId",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "AboutStore",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "ContactUS",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "IsFreeSender",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "IsPayInLocation",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "IsPaybyUser",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "PriceAsStore",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "PriceFree",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "PriceOtherProvince",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "Transportation",
                table: "Setting");
        }
    }
}
