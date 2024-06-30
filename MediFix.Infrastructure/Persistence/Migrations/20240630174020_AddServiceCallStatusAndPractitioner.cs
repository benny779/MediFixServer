using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediFix.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceCallStatusAndPractitioner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PractitionerId",
                table: "ServiceCalls",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "ServiceCalls",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCalls_PractitionerId",
                table: "ServiceCalls",
                column: "PractitionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCalls_Practitioners_PractitionerId",
                table: "ServiceCalls",
                column: "PractitionerId",
                principalTable: "Practitioners",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCalls_Practitioners_PractitionerId",
                table: "ServiceCalls");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCalls_PractitionerId",
                table: "ServiceCalls");

            migrationBuilder.DropColumn(
                name: "PractitionerId",
                table: "ServiceCalls");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ServiceCalls");
        }
    }
}
