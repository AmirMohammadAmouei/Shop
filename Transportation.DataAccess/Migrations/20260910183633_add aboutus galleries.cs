using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transportation.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addaboutusgalleries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUsGallery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutUsId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdtedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsGallery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutUsGallery_AboutUs_AboutUsId",
                        column: x => x.AboutUsId,
                        principalTable: "AboutUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsGallery_AboutUsId",
                table: "AboutUsGallery",
                column: "AboutUsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUsGallery");
        }
    }
}
