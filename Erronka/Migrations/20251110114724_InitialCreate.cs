using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Erronka.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Erabiltzaileak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Izena = table.Column<string>(type: "text", nullable: false),
                    Pasahitza = table.Column<string>(type: "text", nullable: false),
                    Rola = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erabiltzaileak", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Erabiltzaileak",
                columns: new[] { "Id", "izena", "pasahitza", "rola" },
                values: new object[,]
                {
                    { 1, "izena", "1234", "admin" },
                    { 2, "user", "1234", "erabiltzailea" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Erabiltzaileak");
        }
    }
}
