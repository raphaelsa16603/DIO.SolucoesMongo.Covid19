using Microsoft.EntityFrameworkCore;
using CovidBrDataSetFileProcess.Model;

namespace CovidBrDataSetFileProcess.Model
{
    public class Context : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DadosCovid19BR.db");
        }

        public DbSet<DadosCovid> OsDadosDoCovid { get; set; }

    }
}