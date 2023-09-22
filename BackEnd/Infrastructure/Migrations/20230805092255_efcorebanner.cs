using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class efcorebanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerOneRightUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BannerOneRightId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BannerOneLeftUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BannerOneLeftId = table.Column<int>(type: "int", nullable: true),
                    BannerTwoUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BannerTwoId = table.Column<int>(type: "int", nullable: true),
                    BannerThreeUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BannerThreeId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banner_File_BannerOneLeftId",
                        column: x => x.BannerOneLeftId,
                        principalTable: "File",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Banner_File_BannerOneRightId",
                        column: x => x.BannerOneRightId,
                        principalTable: "File",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Banner_File_BannerThreeId",
                        column: x => x.BannerThreeId,
                        principalTable: "File",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Banner_File_BannerTwoId",
                        column: x => x.BannerTwoId,
                        principalTable: "File",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banner_BannerOneLeftId",
                table: "Banner",
                column: "BannerOneLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_BannerOneRightId",
                table: "Banner",
                column: "BannerOneRightId");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_BannerThreeId",
                table: "Banner",
                column: "BannerThreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_BannerTwoId",
                table: "Banner",
                column: "BannerTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_Guid",
                table: "Banner",
                column: "Guid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner");
        }
    }
}
