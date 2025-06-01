using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DashboardWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BussinessDayTypeSet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BussinessDayTypeSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskSet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskNumber = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    InitiatorName = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ExecutorName = table.Column<string>(type: "text", nullable: false),
                    ExecutedTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    StartTaskDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    EndTaskDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()"),
                    CreateAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BussinessDaySet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BussinessDaySet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BussinessDaySet_BussinessDayTypeSet_TypeId",
                        column: x => x.TypeId,
                        principalTable: "BussinessDayTypeSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BussinessDayTypeSet",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1L, "Рабочий" },
                    { 2L, "Выходной" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BussinessDaySet_TypeId",
                table: "BussinessDaySet",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BussinessDaySet");

            migrationBuilder.DropTable(
                name: "TaskSet");

            migrationBuilder.DropTable(
                name: "BussinessDayTypeSet");
        }
    }
}
