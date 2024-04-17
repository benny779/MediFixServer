using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLocationUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Name_LocationType_ParentId",
                table: "Locations");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name_LocationType_ParentId",
                table: "Locations",
                columns: new[] { "Name", "LocationType", "ParentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Name_LocationType_ParentId",
                table: "Locations");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name_LocationType_ParentId",
                table: "Locations",
                columns: new[] { "Name", "LocationType", "ParentId" },
                unique: true,
                filter: "[ParentId] IS NOT NULL");
        }
    }
}
