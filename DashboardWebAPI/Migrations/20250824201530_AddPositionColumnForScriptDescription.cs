using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionColumnForScriptDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "ScriptDescriptionNoteSet",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "ScriptDescriptionNoteSet");
        }
    }
}
