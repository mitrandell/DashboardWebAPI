using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScriptDescriptionNoteSet_ScriptNoteSet_ScriptNoteId",
                table: "ScriptDescriptionNoteSet");

            migrationBuilder.AlterColumn<long>(
                name: "ScriptNoteId",
                table: "ScriptDescriptionNoteSet",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScriptDescriptionNoteSet_ScriptNoteSet_ScriptNoteId",
                table: "ScriptDescriptionNoteSet",
                column: "ScriptNoteId",
                principalTable: "ScriptNoteSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScriptDescriptionNoteSet_ScriptNoteSet_ScriptNoteId",
                table: "ScriptDescriptionNoteSet");

            migrationBuilder.AlterColumn<long>(
                name: "ScriptNoteId",
                table: "ScriptDescriptionNoteSet",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ScriptDescriptionNoteSet_ScriptNoteSet_ScriptNoteId",
                table: "ScriptDescriptionNoteSet",
                column: "ScriptNoteId",
                principalTable: "ScriptNoteSet",
                principalColumn: "Id");
        }
    }
}
