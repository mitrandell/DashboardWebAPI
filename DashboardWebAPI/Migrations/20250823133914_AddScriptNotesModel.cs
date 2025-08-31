using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DashboardWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddScriptNotesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScriptNoteSet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptNoteSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScriptDescription",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScriptNoteId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScriptDescription_ScriptNoteSet_ScriptNoteId",
                        column: x => x.ScriptNoteId,
                        principalTable: "ScriptNoteSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScriptDescription_ScriptNoteId",
                table: "ScriptDescription",
                column: "ScriptNoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScriptDescription");

            migrationBuilder.DropTable(
                name: "ScriptNoteSet");
        }
    }
}
