using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidBrDataSetFileProcess.Migrations
{
    public partial class InitDadosCovid19BR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OsDadosDoCovid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    city = table.Column<string>(type: "TEXT", nullable: true),
                    city_ibge_code = table.Column<string>(type: "TEXT", nullable: true),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    epidemiological_week = table.Column<string>(type: "TEXT", nullable: true),
                    estimated_population = table.Column<long>(type: "INTEGER", nullable: false),
                    estimated_population_2019 = table.Column<long>(type: "INTEGER", nullable: false),
                    is_last = table.Column<string>(type: "TEXT", nullable: true),
                    is_repeated = table.Column<string>(type: "TEXT", nullable: true),
                    city_ibglast_available_confirmede_code = table.Column<long>(type: "INTEGER", nullable: false),
                    last_available_confirmed_per_100k_inhabitants = table.Column<double>(type: "REAL", nullable: false),
                    last_available_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    last_available_death_rate = table.Column<double>(type: "REAL", nullable: false),
                    last_available_deaths = table.Column<long>(type: "INTEGER", nullable: false),
                    order_for_place = table.Column<long>(type: "INTEGER", nullable: false),
                    place_type = table.Column<string>(type: "TEXT", nullable: true),
                    state = table.Column<string>(type: "TEXT", nullable: true),
                    new_confirmed = table.Column<long>(type: "INTEGER", nullable: false),
                    new_deaths = table.Column<long>(type: "INTEGER", nullable: false),
                    uId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsDadosDoCovid", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OsDadosDoCovid");
        }
    }
}
