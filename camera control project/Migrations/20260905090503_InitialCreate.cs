using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace camera_control_project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrafficEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlateNo = table.Column<string>(type: "TEXT", nullable: false),
                    CameraId = table.Column<int>(type: "INTEGER", nullable: false),
                    Speed = table.Column<double>(type: "REAL", nullable: false),
                    MaxSpeed = table.Column<double>(type: "REAL", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficEvents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrafficEvents");
        }
    }
}
