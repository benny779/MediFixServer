using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFix.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClodeDetailsToServiceCall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloseDetails",
                table: "ServiceCalls",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseDetails",
                table: "ServiceCalls");
        }
    }
}
