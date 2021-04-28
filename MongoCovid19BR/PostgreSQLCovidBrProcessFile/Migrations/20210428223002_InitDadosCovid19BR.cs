using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PostgreSQLCovidBrProcessFile.Migrations
{
    public partial class InitDadosCovid19BR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OsDadosDoCovid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city = table.Column<string>(type: "text", nullable: true),
                    city_ibge_code = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    epidemiological_week = table.Column<string>(type: "text", nullable: true),
                    estimated_population = table.Column<long>(type: "bigint", nullable: false),
                    estimated_population_2019 = table.Column<long>(type: "bigint", nullable: false),
                    is_last = table.Column<string>(type: "text", nullable: true),
                    is_repeated = table.Column<string>(type: "text", nullable: true),
                    city_ibglast_available_confirmede_code = table.Column<long>(type: "bigint", nullable: false),
                    last_available_confirmed_per_100k_inhabitants = table.Column<double>(type: "double precision", nullable: false),
                    last_available_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_available_death_rate = table.Column<double>(type: "double precision", nullable: false),
                    last_available_deaths = table.Column<long>(type: "bigint", nullable: false),
                    order_for_place = table.Column<long>(type: "bigint", nullable: false),
                    place_type = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    new_confirmed = table.Column<long>(type: "bigint", nullable: false),
                    new_deaths = table.Column<long>(type: "bigint", nullable: false),
                    uId = table.Column<string>(type: "text", nullable: true),
                    dadosNovos = table.Column<bool>(type: "boolean", nullable: false),
                    dadosAtualizados = table.Column<bool>(type: "boolean", nullable: false)
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
