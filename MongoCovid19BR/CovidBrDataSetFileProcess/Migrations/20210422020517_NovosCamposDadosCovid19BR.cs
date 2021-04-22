using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidBrDataSetFileProcess.Migrations
{
    public partial class NovosCamposDadosCovid19BR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "dadosAtualizados",
                table: "OsDadosDoCovid",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "dadosNovos",
                table: "OsDadosDoCovid",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dadosAtualizados",
                table: "OsDadosDoCovid");

            migrationBuilder.DropColumn(
                name: "dadosNovos",
                table: "OsDadosDoCovid");
        }
    }
}
