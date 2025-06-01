using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNullableCriticalTaskValueSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ActionStatus",
                table: "CriticalTaskSet",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "В процессе");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ActionStatus",
                table: "CriticalTaskSet",
                type: "text",
                nullable: false,
                defaultValue: "В процессе",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
