using Microsoft.EntityFrameworkCore;
using MsSqlCovidBrFileProcess.Model;
using Microsoft.Extensions.DependencyInjection;

namespace MsSqlCovidBrFileProcess.Model
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
            optionsBuilder
            //.EnableSensitiveDataLogging()
            .UseSqlServer(
               @"Server=.;Database=Covid19Br;" +
                "Integrated Security=true;"); // "userid=appcovid19br;password=appcovid19br;"); 
            //,providerOptions => { providerOptions.EnableRetryOnFailure(); });

        }

        public DbSet<DadosCovid> OsDadosDoCovid { get; set; }

        public DbSet<Estado> Estados { get; set; }

        public DbSet<Cidade> Cidades { get; set; }

    }
}