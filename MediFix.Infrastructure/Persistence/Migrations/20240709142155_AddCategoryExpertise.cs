using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFix.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryExpertise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryExpertise",
                columns: table => new
                {
                    AllowedExpertisesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryExpertise", x => new { x.AllowedExpertisesId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_CategoryExpertise_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryExpertise_Expertises_AllowedExpertisesId",
                        column: x => x.AllowedExpertisesId,
                        principalTable: "Expertises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryExpertise_CategoriesId",
                table: "CategoryExpertise",
                column: "CategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryExpertise");
        }
    }
}
