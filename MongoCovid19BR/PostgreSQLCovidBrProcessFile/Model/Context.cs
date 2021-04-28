using Microsoft.EntityFrameworkCore;
using PostgreSQLCovidBrProcessFile.Model;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace PostgreSQLCovidBrProcessFile.Model
{
    public class Context : DbContext
    {
        /*

        A string de conexão a ser definida para fazer o acesso será : 
        Host=localhost;Port=5432;Pooling=true;Database=Cadastro;User Id=??;Password=??;
        */

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=DadosCovid19BR.db");
            optionsBuilder.UseNpgsql(
               "Host=raphaelsa-desktop-n3.local;Port=5432;Pooling=true;Database=Covid19Br;User Id=appCovid19Br;Password=covid19Br;");
               
        }

        public DbSet<DadosCovid> OsDadosDoCovid { get; set; }

    }
}