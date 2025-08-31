using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddScriptDescriptionNoteSetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScriptDescription_ScriptNoteSet_ScriptNoteId",
                table: "ScriptDescription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScriptDescription",
                table: "ScriptDescription");

            migrationBuilder.RenameTable(
                name: "ScriptDescription",
                newName: "ScriptDescriptionNoteSet");

            migrationBuilder.RenameIndex(
                name: "IX_ScriptDescription_ScriptNoteId",
                table: "ScriptDescriptionNoteSet",
                newName: "IX_ScriptDescriptionNoteSet_ScriptNoteId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "ScriptDescriptionNoteSet",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScriptDescriptionNoteSet",
                table: "ScriptDescriptionNoteSet",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScriptDescriptionNoteSet_ScriptNoteSet_ScriptNoteId",
                table: "ScriptDescriptionNoteSet",
                column: "ScriptNoteId",
                principalTable: "ScriptNoteSet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScriptDescriptionNoteSet_ScriptNoteSet_ScriptNoteId",
                table: "ScriptDescriptionNoteSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScriptDescriptionNoteSet",
                table: "ScriptDescriptionNoteSet");

            migrationBuilder.RenameTable(
                name: "ScriptDescriptionNoteSet",
                newName: "ScriptDescription");

            migrationBuilder.RenameIndex(
                name: "IX_ScriptDescriptionNoteSet_ScriptNoteId",
                table: "ScriptDescription",
                newName: "IX_ScriptDescription_ScriptNoteId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "ScriptDescription",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScriptDescription",
                table: "ScriptDescription",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScriptDescription_ScriptNoteSet_ScriptNoteId",
                table: "ScriptDescription",
                column: "ScriptNoteId",
                principalTable: "ScriptNoteSet",
                principalColumn: "Id");
        }
    }
}
