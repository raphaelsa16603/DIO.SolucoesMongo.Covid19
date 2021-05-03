using Microsoft.EntityFrameworkCore;
using IncrementalDataFileProcessSQLite.Model;

namespace IncrementalDataFileProcessSQLite.Model
{
    public class Context : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=../CovidBrDataSetFileProcess/DadosCovid19BR.db");
        }

        public DbSet<DadosCovid> OsDadosDoCovid { get; set; }

    }
}