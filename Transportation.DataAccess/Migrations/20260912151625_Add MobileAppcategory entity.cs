using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transportation.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMobileAppcategoryentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "MobileApps",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MobileAppCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdtedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileAppCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileAppCategories_MobileAppCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MobileAppCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MobileApps_CategoryId",
                table: "MobileApps",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileAppCategories_ParentId",
                table: "MobileAppCategories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileApps_MobileAppCategories_CategoryId",
                table: "MobileApps",
                column: "CategoryId",
                principalTable: "MobileAppCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobileApps_MobileAppCategories_CategoryId",
                table: "MobileApps");

            migrationBuilder.DropTable(
                name: "MobileAppCategories");

            migrationBuilder.DropIndex(
                name: "IX_MobileApps_CategoryId",
                table: "MobileApps");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MobileApps");
        }
    }
}
