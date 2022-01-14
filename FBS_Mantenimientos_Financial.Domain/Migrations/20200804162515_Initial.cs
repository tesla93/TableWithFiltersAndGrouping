using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace FBS_Mantenimientos_Financial.Domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreSchemma = table.Column<string>(nullable: true),
                    NombreTabla = table.Column<string>(nullable: true),
                    CodigoUsuario = table.Column<string>(nullable: true),
                    Llaves = table.Column<string>(nullable: true),
                    ViejosValores = table.Column<string>(nullable: true),
                    NuevosValores = table.Column<string>(nullable: true),
                    FechaMaquina = table.Column<DateTime>(nullable: false),
                    FechaSistema = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audits");
        }
    }
}