using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarea6.Migrations
{
    /// <inheritdoc />
    public partial class refrs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regis",
                columns: table => new
                {
                    IdR = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    refreshtoken1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenExpired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regis", x => x.IdR);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaN = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Regis");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
