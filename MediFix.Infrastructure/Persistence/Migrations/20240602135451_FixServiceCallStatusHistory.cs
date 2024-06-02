using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediFix.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixServiceCallStatusHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceCallStatusUpdate",
                table: "ServiceCallStatusUpdate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCallStatusUpdate_ServiceCallId",
                table: "ServiceCallStatusUpdate");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ServiceCallStatusUpdate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceCallStatusUpdate",
                table: "ServiceCallStatusUpdate",
                columns: new[] { "ServiceCallId", "DateTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceCallStatusUpdate",
                table: "ServiceCallStatusUpdate");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ServiceCallStatusUpdate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceCallStatusUpdate",
                table: "ServiceCallStatusUpdate",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCallStatusUpdate_ServiceCallId",
                table: "ServiceCallStatusUpdate",
                column: "ServiceCallId");
        }
    }
}
