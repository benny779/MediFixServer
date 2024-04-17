using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Location_ParentId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCalls_Location_LocationId",
                table: "ServiceCalls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Location",
                table: "Location");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Locations");

            migrationBuilder.RenameIndex(
                name: "IX_Location_ParentId",
                table: "Locations",
                newName: "IX_Locations_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Location_Name_LocationType_ParentId",
                table: "Locations",
                newName: "IX_Locations_Name_LocationType_ParentId");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_ParentId",
                table: "Locations",
                column: "ParentId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCalls_Locations_LocationId",
                table: "ServiceCalls",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCalls_Locations_LocationId",
                table: "ServiceCalls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Locations");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Location");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_ParentId",
                table: "Location",
                newName: "IX_Location_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_Name_LocationType_ParentId",
                table: "Location",
                newName: "IX_Location_Name_LocationType_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Location",
                table: "Location",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Location_ParentId",
                table: "Location",
                column: "ParentId",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCalls_Location_LocationId",
                table: "ServiceCalls",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
