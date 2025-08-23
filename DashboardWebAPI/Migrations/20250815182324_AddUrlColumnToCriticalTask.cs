using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlColumnToCriticalTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlToRedmineTask",
                table: "CriticalTaskSet",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlToRedmineTask",
                table: "CriticalTaskSet");
        }
    }
}
