using System;
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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Izena = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Pasahitza = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Rola = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erabiltzaileak", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "erreserbak",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mahi_id = table.Column<int>(type: "integer", nullable: true),
                    erabiltzaile_id = table.Column<int>(type: "integer", nullable: true),
                    data = table.Column<DateTime>(type: "date", nullable: false),
                    mota = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    izena = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_erreserbak", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "produktuak",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    izena = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prezioa = table.Column<decimal>(type: "numeric", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    kategoria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produktuak", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Erabiltzaileak",
                columns: new[] { "id", "Izena", "Pasahitza", "Rola" },
                values: new object[,]
                {
                    { 1, "admin", "1234", "admin" },
                    { 2, "usuario", "1234", "user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Erabiltzaileak");

            migrationBuilder.DropTable(
                name: "erreserbak");

            migrationBuilder.DropTable(
                name: "produktuak");
        }
    }
}
