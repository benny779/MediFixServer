using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFix.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSubCategory_IsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SubCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SubCategories");
        }
    }
}
